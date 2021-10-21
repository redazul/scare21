using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, ISpawnable
{
    private bool wasSpawned = false;
    public void SetSpawned(bool wasSpawned)
    {
        this.wasSpawned = wasSpawned;
    }

    void OnTriggerEnter(Collider triggerCol)
    {
        if (triggerCol.CompareTag(PlayerController.PLAYER_TAG))
        {
            PlayerController player = triggerCol.GetComponent<PlayerController>();
            if (player != null) player.GetHit(1, transform);
        }
    }
}