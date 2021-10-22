using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSoundPlayer : MonoBehaviour
{
    private AudioSource soundAudioSource;

    [SerializeField]
    private List<AudioClip> environmentClips;

    [SerializeField]
    private float minTimeUntilRandomClip = 8f;

    [SerializeField]
    private float maxTimeUntilRandomClip = 17f;
    private Timer noAudioPlayedTimer;

    private bool isRunning = false;

    void Awake()
    {
        soundAudioSource = GetComponent<AudioSource>();
        soundAudioSource.loop = false;

        noAudioPlayedTimer = gameObject.AddComponent<Timer>();
    }

    public void PlayAudioClip(AudioClip clip)
    {
        soundAudioSource.Stop();
        soundAudioSource.clip = clip;
        RestartRandomClipTimer();
        soundAudioSource.Play();
    }

    private void RestartRandomClipTimer()
    {
        noAudioPlayedTimer.Init(Random.Range(minTimeUntilRandomClip, maxTimeUntilRandomClip),
            delegate
            {
                if (isRunning)
                {
                    PlayAudioClip(PlSoundPlayer.GetRandomFromList(environmentClips));
                }
            });
    }

    private void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
        if (isRunning)
        {
            RestartRandomClipTimer();
        }
    }

    public void SetSoundVolume(float volume)
    {
        soundAudioSource.volume = volume;
    }
}
