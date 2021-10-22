using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, ISpawnable
{
    [SerializeField]
    private float timeToSnap = 0.5f;

    [SerializeField]
    private List<AudioClip> triggerHitClips;

    [SerializeField]
    private List<AudioClip> triggerMissClips;

    private bool wasSpawned = false;

    PlayerController player;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetSpawned(bool wasSpawned)
    {
        this.wasSpawned = wasSpawned;
    }

    void OnTriggerEnter(Collider triggerCol)
    {
        if (triggerCol.CompareTag(PlayerController.PLAYER_TAG))
        {
            player = triggerCol.gameObject.transform.root.GetComponent<PlayerController>();
            InitTimedTrapTrigger();
        }
    }

    private void OnTriggerExit(Collider triggerCol)
    {
        if (triggerCol.CompareTag(PlayerController.PLAYER_TAG))
        {
            player = null;
        }
    }

    private void InitTimedTrapTrigger()
    {
        Timer timer = this.gameObject.AddComponent<Timer>();
        timer.Init(timeToSnap, TriggerTrap);
    }

    private void TriggerTrap()
    {
        if (player != null)
        {
            player.GetHit(1, transform);
            PlayAudioClip(PlSoundPlayer.GetRandomFromList(triggerHitClips));
        }
        else
        {
            PlayAudioClip(PlSoundPlayer.GetRandomFromList(triggerMissClips));
        }
    }

    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SetSoundVolume(float volume)
    {
        audioSource.volume = volume;
    }
}