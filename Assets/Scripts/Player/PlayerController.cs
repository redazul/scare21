using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const string PLAYER_TAG = "Player";

    private const float MAX_XZ_ROTATION_ANGLE = 30f;

    private const float INTERACTION_RADIUS = 0.45f;
    private const string INTERACTABLE_TAG = "Interactable";

    [Tooltip("the maximum amount of health that the player can have")]
    [SerializeField]
    private int maxHealth = 3;

    [Tooltip("the (initial) health of the player")]
    [SerializeField]
    private int health = 3;

    [Tooltip("maximum amount of cheese that can be carried")]
    [SerializeField]
    private float maxCheeseAmount = 10.0f;

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

    [Tooltip("If the player returns to original angle")]
    [SerializeField]
    private float angleCorrectionPerSec = 5f;



    //[Tooltip("How fast the move corrects rotation based on terrain movement (surface normals)")]
    //[SerializeField]
    //float rotateCorrectionSpeed = 50f;

    [SerializeField]
    private bool displayInteractionSphereGizmo = true;

    [SerializeField]
    private GameObject interactionPosition;

    [SerializeField]
    private Transform mushroomRoot;

    [SerializeField]
    private Light mushroomTargetLight;

    private float carriedCheese = 0f;
    private float currentMovementSpeed = 1.0f;
    //private float groundCheckDistance = 1.5f;

    private Rigidbody rigidBody;
    private AnimationControl animationControl;

    private bool isDead = false;
    //the current cheese that the mouse carries

    //Mushroom
    Mushroom mushroom;

    private static float cheeseCapacity = 10.0f;

    private bool useCharController;

    void Awake()
    {
        useCharController = GetComponent<CharacterController>() != null;
        if (!useCharController)
        {
            Debug.LogWarning("character controller component not found on player. using custom movement");
        }
        rigidBody = GetComponent<Rigidbody>();
        animationControl = GetComponentInChildren<AnimationControl>();

        UpdateMovementSpeedFromCheeseAmount();
        if (cheesePrefab == null)
        {
            Debug.LogWarning("Cheese prefab was not set so no cheese will be instantiated when the player drops the cheese.");
        }

        //make cheeseCapacity availabe for static getter access
        cheeseCapacity = maxCheeseAmount;
    }

    void Start()
    {
        References.SetPaused(false);
    }

    private void Update()
    {
        ProcessPauseInteractions();
        if (References.GetPaused()) return;

        ProcessInteractions();
    }

    void FixedUpdate()
    {
        if (References.GetPaused()) return;

        ProcessMovement();
        ApplyAngleLimits();
    }

    public void GetHit(float attackPushMagnitude, Transform other)
    {
        if (attackPushMagnitude > 0.0f)
        {
            Vector3 pushForce = (transform.position - other.position).normalized * attackPushMagnitude;
            pushForce.y = 0.5f;
            GetComponent<Rigidbody>()?.AddForce(pushForce, ForceMode.Impulse);
        }
    }

    private void ProcessMovement()
    {
        if (useCharController)
        {
            return;
        }
        //cant move if you are dead
        if (isDead)
        {
            return;
        }

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
            animationControl?.SetMovementSpeed(0);
            return;
        }
        Move(movement * Time.fixedDeltaTime * currentMovementSpeed);
        animationControl?.SetMovementSpeed(currentMovementSpeed);
    }

    private void Move(Vector3 movement)
    {
        rigidBody.MovePosition(transform.position + movement);
    }

    private void ProcessInteractions()
    {
        //cant interact if you are dead
        if (isDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bool interacted = TryToInteract();
            if (!interacted && mushroom)
            {
                if (mushroom) mushroom.Detach();
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

    private void ProcessPauseInteractions()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMenu menu = References.GetGameMenu();
            if (menu != null)
            {
                int currentMenu = menu.GetMenu();

                if (currentMenu == References.PAUSE)
                {
                    DeactivateAllMenus();
                    References.SetPaused(false);
                }
                else if (currentMenu == References.OPTIONS)
                {
                    SetMenu(References.PAUSE);
                }
                else if (currentMenu == -1)
                {
                    SetMenu(References.PAUSE);
                    References.SetPaused(true);
                }
            }
        }
    }

    /// <summary>
    /// Tries to interact with the objects around the interaction position.
    /// </summary>
    /// <returns>True, if the player interacted with anything</returns>
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
    public void AddCheese(float amountToAdd)
    {
        ChangeCheeseAmount(amountToAdd);
    }

    private void RemoveCheese()
    {
        //don't drop anything if you dont have any cheese
        if (carriedCheese <= 0)
        {
            return;
        }

        float amountToDrop;
        //make sure you don't drop too much cheese
        if (carriedCheese <= Cheese.MIN_CHEESE_AMOUNT * 2)
        {
            amountToDrop = carriedCheese;
        } else if(carriedCheese <= Cheese.MAX_CHEESE_AMOUNT * 2)
        {
            amountToDrop = carriedCheese / 2;
        } else
        {
            amountToDrop = Mathf.Min(Cheese.GetRandomCheeseAmount(), carriedCheese);
        }

        //spawn the cheese
        GameObject spawnedCheeseObject = SpawnDroppedItem(cheesePrefab);
        spawnedCheeseObject.GetComponent<Cheese>().SetAmount(amountToDrop);

        //update carried cheese amount
        ChangeCheeseAmount(-amountToDrop);
    }

    private GameObject SpawnDroppedItem(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("prefab to be spawned when an item should be dropped is null. Make sure its assigned in the inspector.");
        }
        Quaternion targetRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

        return Instantiate(prefab, GetDisplacedDropPosition(), targetRotation);
    }

    private void ChangeCheeseAmount(float change)
    {
        carriedCheese = Mathf.Clamp(carriedCheese + change, 0, maxCheeseAmount);
        UpdateMovementSpeedFromCheeseAmount();
        HUDManager.Instance.UpdateCheeseAmount(carriedCheese);
    }

    public float GetCarriedCheeseAmount()
    {
        return carriedCheese;
    }

    public void RemoveAllCheese()
    {
        carriedCheese = 0;
        UpdateMovementSpeedFromCheeseAmount();
        HUDManager.Instance.UpdateCheeseAmount(carriedCheese);
    }

    public static float GetCheeseCapacity()
    {
        return cheeseCapacity;
    }

    public void HoldMushroom(Mushroom m)
    {
        mushroom = m;
    }

    public Vector3 DropMushroom()
    {
        mushroom = null;
        return GetDisplacedDropPosition();
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

        if (isDead)
        {
            if (useCharController)
            {
                GetComponent<ThirdPersonController>().SetDead(true);
                GetComponent<CharacterController>().enabled = false;
            }
            SetMenu(References.GAME_OVER);
        }
    }

    void SetMenu(int menuIndex)
    {
        GameMenu menu = References.GetGameMenu();
        if (menu != null)
        {
            menu.SetMenu(menuIndex);
        }
    }

    void DeactivateAllMenus()
    {
        GameMenu menu = References.GetGameMenu();
        if (menu != null)
        {
            menu.DeactivateAllMenus();
        }
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

        if(FloatAlmostZero(currentXAngle, 0.01f) && FloatAlmostZero(currentZAngle, 0.01f))
        {
            return;
        }

        if (currentXAngle > MAX_XZ_ROTATION_ANGLE && currentXAngle < 360 - MAX_XZ_ROTATION_ANGLE)
        {
            currentXAngle = currentXAngle > 180 ? 360 - MAX_XZ_ROTATION_ANGLE : MAX_XZ_ROTATION_ANGLE;
        }
        if (currentZAngle > MAX_XZ_ROTATION_ANGLE && currentZAngle < 360 - MAX_XZ_ROTATION_ANGLE)
        {
            currentZAngle = currentZAngle > 180 ? 360 - MAX_XZ_ROTATION_ANGLE : MAX_XZ_ROTATION_ANGLE;
        }

        transform.eulerAngles = new Vector3(GetNextAngleToZero(currentXAngle, angleCorrectionPerSec), currentYAngle, GetNextAngleToZero(currentZAngle, angleCorrectionPerSec));
    }

    private float GetNextAngleToZero(float currentAngle, float correctionPerTime)
    {
        float correction;
        if (FloatAlmostZero(currentAngle, 0.1f))
        {
            return 0.0f;
        }
        if(currentAngle >= 180)
        {
            correction = Mathf.Min(360 - currentAngle, correctionPerTime * Time.fixedDeltaTime);
            return currentAngle + correction;
        }
        //currentAngle < 180
        correction = Mathf.Max(-currentAngle, -correctionPerTime * Time.fixedDeltaTime);
        return currentAngle + correction;
    }

    private Vector3 GetDisplacedDropPosition()
    {
        Vector3 displacement = transform.right * UnityEngine.Random.Range(0.2f, 0.5f);
        displacement *= (UnityEngine.Random.value > 0.5f) ? 1 : -1;

        return interactionPosition.transform.position + displacement;
    } 

    private static bool FloatAlmostZero(float value, float delta)
    {
        if(value < delta && value > -delta)
        {
            return true;
        }
        return false;
    }

    public Transform GetMushroomRoot()
    {
        return mushroomRoot;
    }

    /// <summary>
    /// Returns a custom light source to control. If null, the mushrooms light will be used
    /// </summary>
    /// <returns></returns>
    public Light GetMushroomTargetLight()
    {
        return mushroomTargetLight;
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
