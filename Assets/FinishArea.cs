using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishArea : MonoBehaviour
{

    void OnTriggerEnter(Collider triggerCol)
    {
        if (triggerCol.CompareTag(PlayerController.PLAYER_TAG))
        {
            PlayerController player = triggerCol.GetComponent<PlayerController>();
            float cheeseCarried = player.GetCarriedCheeseAmount();
            ScoreManager.FinishLevel(cheeseCarried);
        }
    }
}
