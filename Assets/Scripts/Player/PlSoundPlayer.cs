using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlSoundPlayer : MonoBehaviour
{
    private AudioSource soundAudioSource;
    private AudioSource musicAudioSource;

    [SerializeField]
    private List<AudioClip> hurtClips;

    [SerializeField]
    private List<AudioClip> pickUpClips;

    [SerializeField]
    private List<AudioClip> dropClips;

    [SerializeField]
    private List<AudioClip> randomClips;

    [SerializeField]
    private List<AudioClip> catCueMusicClips;

    [SerializeField]
    private List<AudioClip> kitchenMusicClips;

    [SerializeField]
    private bool playRandomVoiceClips = true;

    [SerializeField]
    private float minTimeUntilRandomClip = 25f;

    [SerializeField]
    private float maxTimeUntilRandomClip = 40f;

    private Timer noAudioPlayedTimer;

    private AudioClip backgroundMusicClip;

    public enum PlayerClipType
    {
        hurt, pickUp, drop, random
    }

    public enum PlayerMusicClipType
    {
        catCue, kitchen
    }

    void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        soundAudioSource = audioSources[0];
        soundAudioSource.loop = false;

        musicAudioSource = audioSources[1];
        musicAudioSource.loop = true;

        noAudioPlayedTimer = gameObject.AddComponent<Timer>();
        RestartRandomClipTimer();
    }

    public void PlayAudioClip(PlayerClipType plClip)
    {
        soundAudioSource.Stop();
        soundAudioSource.clip = GetClipFor(plClip);
        RestartRandomClipTimer();
        soundAudioSource.Play();
    }

    public void PlayMusicClip(PlayerMusicClipType plClip, bool isBackgroundMusic)
    {
        if (isBackgroundMusic)
        {
            backgroundMusicClip = musicAudioSource.clip;
        }
        musicAudioSource.Stop();
        musicAudioSource.clip = GetClipFor(plClip);
        musicAudioSource.Play();
    }

    public void StopAndPlayBackgroundMusicClip()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = backgroundMusicClip;
        if(backgroundMusicClip != null)
        {
            musicAudioSource.Play();
        }
    }

    private AudioClip GetClipFor(PlayerClipType clipType)
    {
        switch (clipType)
        {
            case PlayerClipType.hurt:
                {
                    return GetRandomFromList(hurtClips);
                }
            case PlayerClipType.pickUp:
                {
                    return GetRandomFromList(pickUpClips);
                }
            case PlayerClipType.drop:
                {
                    return GetRandomFromList(dropClips);
                }
            case PlayerClipType.random:
                {
                    return GetRandomFromList(randomClips);
                }
        }
        return null;
    }

    private AudioClip GetClipFor(PlayerMusicClipType clipType)
    {
        switch (clipType)
        {
            case PlayerMusicClipType.catCue:
                {
                    return GetRandomFromList(catCueMusicClips);
                }
            case PlayerMusicClipType.kitchen:
                {
                    return GetRandomFromList(kitchenMusicClips);
                }
        }
        return null;
    }

    private void RestartRandomClipTimer()
    {
        if (playRandomVoiceClips)
        {
            noAudioPlayedTimer.Init(Random.Range(minTimeUntilRandomClip, maxTimeUntilRandomClip),
                delegate
                {
                    PlayAudioClip(PlayerClipType.random);
                });
        }
    }

    public static AudioClip GetRandomFromList(List<AudioClip> list)
    {
        if(list.Count == 0)
        {
            Debug.LogError("given list was too small");
        }
        return list[Random.Range(0, list.Count)];
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    } 
    
    public void SetSoundVolume(float volume)
    {
        soundAudioSource.volume = volume;
    }
}
