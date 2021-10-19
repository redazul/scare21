using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MAX_XZ_ROTATION_ANGLE = 30f;

    private const float INTERACTION_RADIUS = 0.45f;
    private const string INTERACTABLE_TAG = "Interactable";

    [Tooltip("the maximum amount of health that the player can have")]
    [SerializeField]
    private int maxHealth = 3;

    [Tooltip("the (initial) health of the player")]
    [SerializeField]
    private int health = 3;

    //[Tooltip("maximum amount of cheese that can be carried")]
    //[SerializeField]
    //private float maxCheeseAmount = float.PositiveInfinity;

    [Tooltip("the amount that will be dropped if the player decides to drop cheese")]
    [SerializeField]
    private float dropCheeseAmount = 1f;

    [Tooltip("The cheese prefab to be instantiated when the player drops the cheese")]
    [SerializeField]
    private GameObject cheesePrefab;

    [Tooltip("Movement speed when the player does not carry cheese")]
    [SerializeField]
    private float baseMovementSpeed = 1.0f;

    [Tooltip("The reduction of movement speed by carried cheese. If this is high, the player gets very slow from picking up cheese. Values should be within 0 and 1")]
    [SerializeField]
    private float movementReductionByCheese = 0.1f;

    [Tooltip("How fast the player turns when using left/right")]
    [SerializeField]
    private float turnSpeedDegreePerSec = 250f;

    //[Tooltip("How fast the move corrects rotation based on terrain movement (surface normals)")]
    //[SerializeField]
    //float rotateCorrectionSpeed = 50f;

    [SerializeField]
    private bool displayInteractionSphereGizmo = true;

    [SerializeField]
    private GameObject interactionPosition;

    public Transform MushroomRoot;

    private float carriedCheese = 0f;
    private float currentMovementSpeed = 1.0f;
    //private float groundCheckDistance = 1.5f;

    private Rigidbody rigidBody;

    private bool isDead = false;
    //the current cheese that the mouse carries

    //Mushroom
    Mushroom mushroom;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        UpdateMovementSpeedFromCheeseAmount();
        if (cheesePrefab == null)
        {
            Debug.LogWarning("Cheese prefab was not set so no cheese will be instantiated when the player drops the cheese.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        ProcessInputs();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessMovement();
        ApplyAngleLimits();
    }

    private void ProcessInputs()
    {
        if (isDead)
        {
            return;
        }
        ProcessInteractions();
    }

    private void ProcessMovement()
    {
        //Vector3 newUp = transform.up, newForward = transform.forward;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        //{
        //    if (hit.transform.name != name)
        //    {
        //        newUp  = hit.normal;
        //    }
        //}

        float turnAngle = Input.GetAxis("Horizontal") * turnSpeedDegreePerSec;
        transform.Rotate(Vector3.up, Time.fixedDeltaTime * turnAngle);

        //Quaternion newRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, newUp), rotateCorrectionSpeed * Time.fixedDeltaTime);
        //transform.rotation = newRot;

        Vector3 movement = transform.forward * Input.GetAxis("Vertical");
        if (movement == Vector3.zero)
        {
            return;
        }
        Move(movement * Time.fixedDeltaTime * currentMovementSpeed);
    }

    private void Move(Vector3 movement)
    {
        rigidBody.MovePosition(transform.position + movement);
    }

    private void ProcessInteractions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!TryToInteract())
            {
                mushroom.Detach();
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            RemoveCheese();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (mushroom) mushroom.ToggleLight();
        }
    }

    private bool TryToInteract()
    {
        Collider[] matchingColliders = Physics.OverlapSphere(interactionPosition.transform.position, INTERACTION_RADIUS);

        foreach (Collider col in matchingColliders)
        {
            if (col.gameObject.CompareTag(INTERACTABLE_TAG))
            {
                col.gameObject.GetComponent<IInteractable>().Interact(transform);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// This should be called when cheese is picked up
    /// </summary>
    public void SetCheese(float newCheeseAmount)
    {
        carriedCheese = newCheeseAmount;
        UpdateMovementSpeedFromCheeseAmount();
    }

    private void RemoveCheese()
    {

        if (carriedCheese <= 0)
        {
            return;
        }
        float amountToDrop = Mathf.Min(dropCheeseAmount, carriedCheese);
        carriedCheese -= amountToDrop;

        if (cheesePrefab != null)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Vector3 displacement = transform.right * Random.Range(0.2f, 0.5f);
            displacement *= (Random.value > 0.5f) ? 1 : -1;

            Vector3 targetPosition = transform.position + transform.forward * 0.5f + displacement;

            GameObject spawnedCheeseObject = Instantiate(cheesePrefab, targetPosition, targetRotation);
        }

        References.SetCheese((int)carriedCheese);
        UpdateMovementSpeedFromCheeseAmount();
    }


    public void HoldMushroom(Mushroom m)
    {
        mushroom = m;
    }


    public Vector3 DropMushroom()
    {
        mushroom = null;
        return interactionPosition.transform.position;
    }


    private void UpdateMovementSpeedFromCheeseAmount()
    {
        currentMovementSpeed = Mathf.Max(0.1f, baseMovementSpeed - carriedCheese * movementReductionByCheese);
    }

    public void ReduceHealth()
    {
        ChangeHealth(-1);
    }

    public void IncreaseHealth()
    {
        ChangeHealth(1);
    }

    private void ChangeHealth(int changeAmount)
    {
        health = Mathf.Clamp(health + changeAmount, 0, maxHealth);

        isDead = health <= 0;
        HUDManager.Instance.UpdateHealthAmount(health);
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void ApplyAngleLimits()
    {
        if (isDead)
        {
            return;
        }
        float currentXAngle = transform.localRotation.eulerAngles.x;
        float currentYAngle = transform.localRotation.eulerAngles.y;
        float currentZAngle = transform.localRotation.eulerAngles.z;

        float targetXAngle = currentXAngle;
        float targetZAngle = currentZAngle;

        bool needsCorrection = false;

        if (currentXAngle > MAX_XZ_ROTATION_ANGLE && currentXAngle < 360 - MAX_XZ_ROTATION_ANGLE)
        {
            targetXAngle = currentXAngle > 180 ? 360 - MAX_XZ_ROTATION_ANGLE : MAX_XZ_ROTATION_ANGLE;
            needsCorrection = true;
        }
        if (currentZAngle > MAX_XZ_ROTATION_ANGLE && currentZAngle < 360 - MAX_XZ_ROTATION_ANGLE)
        {
            targetZAngle = currentZAngle > 180 ? 360 - MAX_XZ_ROTATION_ANGLE : MAX_XZ_ROTATION_ANGLE;
            needsCorrection = true;
        }
        if (needsCorrection)
        {
            transform.eulerAngles = new Vector3(targetXAngle, currentYAngle, targetZAngle);
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!displayInteractionSphereGizmo)
        {
            return;
        }
        Gizmos.color = new Color(0, 1, 1, 0.25f);
        Gizmos.DrawSphere(interactionPosition.transform.position, INTERACTION_RADIUS);
    }

#endif
}
