using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour, IInteractable
{
    [Header("How many seconds of light mushroom has.")]
    public float BatteryLifeSeconds = 10f;

    public bool IsBatteryDepleted { get; private set; }

    [SerializeField]
    Light _light;

    [SerializeField]
    Collider _stemCollider, _headCollider;

    public bool IsLit { get; set; }
    public bool IsAttached { get; set; }

    private Transform originalParent;
    PlayerController holder;
    float batteryLifeLeft;


    private void Awake()
    {
        originalParent = transform.parent;
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
        if (IsLit)
        {
            batteryLifeLeft -= Time.deltaTime;
            if (batteryLifeLeft < 0f)
            {
                batteryLifeLeft = 0f;
                IsBatteryDepleted = true;
                print("Battery depleted!");
            }

            _light.intensity = batteryLifeLeft / BatteryLifeSeconds;
        }
    }


    public void Interact(Transform other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            AttachTo(player.MushroomRoot);
            player.HoldMushroom(this);

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
        _light.enabled = true;
        IsLit = true;
    }

    public void TurnLightOff()
    {
        _light.enabled = false;
        IsLit = false;
    }


    public void ChargeBatteryBySeconds(float seconds)
    {
        batteryLifeLeft += seconds;
        if (batteryLifeLeft > BatteryLifeSeconds) batteryLifeLeft = BatteryLifeSeconds;
    }


    void AttachTo(Transform other)
    {
        _headCollider.enabled = false;
        _stemCollider.enabled = false;

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

        _headCollider.enabled = true;
        _stemCollider.enabled = true;
    }
}
