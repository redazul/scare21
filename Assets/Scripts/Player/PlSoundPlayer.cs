using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private List<AudioClip> hurtClips;

    [SerializeField]
    private List<AudioClip> pickUpClips;

    [SerializeField]
    private List<AudioClip> dropClips;

    [SerializeField]
    private List<AudioClip> dangerHintClips;

    public enum PlayerClipType
    {
        hurt, pickUp, drop, dangerHint
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(PlayerClipType plClip)
    {
        audioSource.Stop();
        audioSource.clip = GetClipFor(plClip);
        audioSource.Play();
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
            case PlayerClipType.dangerHint:
                {
                    return GetRandomFromList(dangerHintClips);
                }
        }
        return null;
    }

    public static AudioClip GetRandomFromList(List<AudioClip> list)
    {
        if(list.Count == 0)
        {
            Debug.LogError("given list was too small");
        }
        return list[Random.Range(0, list.Count)];
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
