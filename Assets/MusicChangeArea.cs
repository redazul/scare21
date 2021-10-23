using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlSoundPlayer;

public class MusicChangeArea : MonoBehaviour
{
    [SerializeField]
    private PlayerMusicClipType musicAreaType = PlayerMusicClipType.outside;


    void OnTriggerEnter(Collider triggerCol)
    {
        if (triggerCol.CompareTag(PlayerController.PLAYER_TAG))
        {
            PlSoundPlayer soundPlayer = triggerCol.transform.root.GetComponentInChildren<PlSoundPlayer>();
            if (soundPlayer.GetActiveMusicType() != musicAreaType)
            {
                soundPlayer.PlayMusicClip(musicAreaType, true);
            }

        }
    }
}
