using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour, IInteractable, ISpawnable
{
    private const float XZ_ROTATION_RANGE = 50f;

    [Header("How many seconds of light mushroom has.")]
    public float BatteryLifeSeconds = 20f;

    public bool IsBatteryDepleted { get; private set; }

    private Light mushroomLight;
    private Light currentUsedLight;

    [SerializeField]
    List<Collider> collidersToDeactivateOnGrab = new List<Collider>();

    public bool IsLit { get; set; }
    public bool IsAttached { get; set; }

    private Transform originalParent;
    PlayerController holder;
    float batteryLifeLeft;

    private bool wasSpawned = false;

    private void Awake()
    {
        originalParent = transform.parent;
        mushroomLight = GetComponentInChildren<Light>();
        currentUsedLight = mushroomLight;
    }

    private void Start()
    {
        batteryLifeLeft = BatteryLifeSeconds;
    }

    // Test driver
    private void Update()
    {
        if (References.GetPaused()) return;

        // "Battery Life"
        if (IsLit && IsAttached)
        {
            batteryLifeLeft -= Time.deltaTime;
            HUDManager.Instance.UpdateMushroomBattery(batteryLifeLeft / BatteryLifeSeconds);

            if (batteryLifeLeft < 0f)
            {
                batteryLifeLeft = 0f;
                IsBatteryDepleted = true;
            }

            currentUsedLight.intensity = batteryLifeLeft / BatteryLifeSeconds;
        }
    }


    public void Interact(Transform other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            AttachTo(player.GetMushroomRoot());
            Light customLight = player.GetMushroomTargetLight();
            if(customLight != null)
            {
                currentUsedLight = customLight;
            }

            player.HoldMushroom(this);
            HUDManager.Instance.SetMushLightHudActive(true);

            holder = player;
        }
    }


    public void ToggleLight()
    {
        if (IsLit) TurnLightOff();
        else TurnLightOn();
    }


    public void TurnLightOn()
    {
        currentUsedLight.enabled = true;
        IsLit = true;
    }

    public void TurnLightOff()
    {
        currentUsedLight.enabled = false;
        IsLit = false;
    }


    public void ChargeBatteryBySeconds(float seconds)
    {
        batteryLifeLeft += seconds;
        if (batteryLifeLeft > BatteryLifeSeconds) batteryLifeLeft = BatteryLifeSeconds;
    }


    void AttachTo(Transform other)
    {
        foreach(Collider c in collidersToDeactivateOnGrab){
            c.enabled = false;
        }

        transform.position = other.position;
        transform.rotation = other.rotation;

        transform.parent = other;
        IsAttached = true;
    }

    public void Detach()
    {
        transform.parent = originalParent;
        IsAttached = false;

        transform.position = holder.DropMushroom();
        holder = null;
        
        transform.localEulerAngles = GetRandomRotation();

        currentUsedLight = mushroomLight;
        HUDManager.Instance.SetMushLightHudActive(false);

        foreach (Collider c in collidersToDeactivateOnGrab)
        {
            c.enabled = true;
        }
    }

    private Vector3 GetRandomRotation()
    {
        float rotX = Random.Range(-XZ_ROTATION_RANGE, XZ_ROTATION_RANGE);
        if (rotX < 0)
        {
            rotX += 360f;
        }
        float rotZ = Random.Range(-XZ_ROTATION_RANGE, XZ_ROTATION_RANGE);
        if (rotZ < 0)
        {
            rotZ += 360f;
        }
        float rotY = Random.Range(0, 360f);

        return new Vector3(rotX, rotY, rotZ);
    }

    public void SetSpawned(bool wasSpawned)
    {
        this.wasSpawned = wasSpawned;
    }
}
