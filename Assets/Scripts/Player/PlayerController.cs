using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MAX_XZ_ROTATION_ANGLE = 30f;

    [Tooltip("maximum amount of cheese that can be carried")]
    [SerializeField]
    private float maxCheeseAmount = float.PositiveInfinity;

    [Tooltip("the amount that will be dropped if the player decides to drop cheese")]
    [SerializeField]
    private float dropCheeseAmount = 0.3f;

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

    [Tooltip("How fast the move corrects rotation based on terrain movement (surface normals)")]
    [SerializeField]
    float rotateCorrectionSpeed = 50f;


    private float carriedCheese = 0f;
    private float currentMovementSpeed = 1.0f;

    private float groundCheckDistance = 1.5f;

    private Rigidbody rigidBody;
    //the current cheese that the mouse carries

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

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessInputs();
        ApplyAngleLimits();
    }

    private void ProcessInputs()
    {
        ProcessMovement();
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            RemoveCheese();
        }
    }

    /// <summary>
    /// This should be called when cheese is picked up
    /// </summary>
    public void AddCheese(float amountToAdd)
    {
        carriedCheese = Mathf.Min(carriedCheese + amountToAdd, maxCheeseAmount);
        UpdateMovementSpeedFromCheeseAmount();
    }

    private void RemoveCheese()
    {
        float amountToDrop = Mathf.Min(dropCheeseAmount, carriedCheese);
        carriedCheese = carriedCheese - amountToDrop;

        if(cheesePrefab != null)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Vector3 displacement = transform.right * Random.Range(0.2f, 0.5f);
            displacement *= (Random.value > 0.5f) ? 1 : -1;

            Vector3 targetPosition = transform.position + transform.forward * 0.5f + displacement;

            GameObject spawnedCheeseObject = Instantiate(cheesePrefab, targetPosition, targetRotation);

            //TODO: set the cheese amount to *amountToDrop*
            //spawnedCheeseObject.GetComponent<Cheese>().SetAmount(amountToDrop);
        }

        UpdateMovementSpeedFromCheeseAmount();
    }

    private void UpdateMovementSpeedFromCheeseAmount()
    {
        currentMovementSpeed = Mathf.Max(0.1f, baseMovementSpeed - carriedCheese * movementReductionByCheese);
    }

    private void ApplyAngleLimits()
    {
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
}
