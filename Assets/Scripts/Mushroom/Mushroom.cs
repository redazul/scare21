using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour, IInteractable
{
    [SerializeField]
    Light _light;

    [SerializeField]
    Collider _stemCollider, _headCollider;

    public bool IsLit { get; set; }
    public bool IsAttached { get; set; }

    private Transform originalParent;
    PlayerController holder;


    private void Awake()
    {
        originalParent = transform.parent;
    }



    // Test driver
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
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
