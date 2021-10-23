using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointArea : MonoBehaviour
{
    public const string PP_KEY_CHECKPOINT_ID = "CheckpointId";

    [Tooltip("The gameobject that will be removed once the player provides the cheese")]
    [SerializeField]
    GameObject barrierToDespawn;

    [Tooltip("How much cheese is needed for the barrier to despawn.")]
    [SerializeField]
    private float cheeseNeeded;

    [Tooltip("If active, this position acts as a checkpoint for the player to respawn once he dies.")]
    [SerializeField]
    private bool isCheckpoint;

    private int checkpointId  = 1;

    public void SetId(int id)
    {
        this.checkpointId = id;
    }

    private bool barrierActive = true;

    void OnTriggerEnter(Collider triggerCol)
    {
        if (barrierActive && triggerCol.CompareTag(PlayerController.PLAYER_TAG))
        {
            PlayerController player = triggerCol.GetComponent<PlayerController>();
            PromptPlayer(player);
        }
    }

    private void PromptPlayer(PlayerController player)
    {
        player.StartCheesePrompt(this, cheeseNeeded);
    }

    private void OnTriggerExit(Collider other)
    {
        if (barrierActive && other.CompareTag(PlayerController.PLAYER_TAG))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            player.StopCheesePrompt();
        }
    }

    public void SetBarrierActive(bool isActive)
    {
        this.barrierActive = isActive;
        if (isActive)
        {
            barrierToDespawn.SetActive(true);
        }
        if (!isActive)
        {
            barrierToDespawn.SetActive(false);

            if (!isCheckpoint)
            {
                return;
            }
            if (checkpointId == -1)
            {
                Debug.LogWarning("Checkpoint area has no valid id yet. Did you forget to add a CheckPointManager?");
            }
            else
            {
                PlayerPrefs.SetInt(PP_KEY_CHECKPOINT_ID, checkpointId);
            }

        }
    }

    public float GetCheeseNeeded()
    {
        return cheeseNeeded;
    }

}
