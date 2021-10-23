using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [Tooltip("Toggles whether respawning is active. This will be called once on Awake")]
    [SerializeField]
    private bool respawnActive = true;

    void Awake()
    {
        if (respawnActive)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if(CheckpointManager.Instance == null)
        {
            Debug.LogWarning("Can't respawn because no checkpointmanager is present in the scene");
            return;
        }
        CheckPointArea targetToRespawn = CheckpointManager.Instance.GetCurrentCheckpoint();
        if(targetToRespawn != null)
        {
            gameObject.transform.root.transform.position = targetToRespawn.gameObject.transform.position;
        }
    }

}
