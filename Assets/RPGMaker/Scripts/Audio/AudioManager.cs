using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioSource> activeAudioSources;
    //private List<AudioSource> audioSources;

    [SerializeField] private Transform soundObject;
    [SerializeField] private int maxAudioSources;
    [SerializeField] [Tooltip("How many of the same sound can be played at once")]
    private int maxSameSound;
    [SerializeField] private AudioClip sound;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartCoroutine(SoundLoop());
    }

    //AudioClip clip, float volume = default
    public void PlaySound(AudioClip clip = null, float volume = 1)
    {
        if (activeAudioSources.Count == 0 || AudioClipCount(clip) < maxSameSound)
        {
            AudioSource newAudioSource = soundObject.gameObject.AddComponent<AudioSource>();
            activeAudioSources.Add(newAudioSource);

            newAudioSource.clip = clip;
            newAudioSource.volume = volume;
            newAudioSource.Play();

            StartCoroutine(StopSound(clip, clip.length));
        }
        else
        {
            AudioSource newAudioSource = GetAudioClipSources(clip)[0];

            newAudioSource.clip = clip;
            newAudioSource.volume = volume;
            newAudioSource.Play();

            StartCoroutine(StopSound(clip, clip.length));
        }
    }

    private int AudioClipCount(AudioClip clip)
    {
        int clipCount = 0;

        for (int sourceIndex = 0; sourceIndex < activeAudioSources.Count; sourceIndex++)
        {
            AudioSource audioSource = activeAudioSources[sourceIndex];

            if (audioSource.clip == clip)
            {
                clipCount++;
            }
        }

        return clipCount;
    }

    private bool IsAudioClipPlaying(AudioClip clip)
    {
        for (int sourceIndex = 0; sourceIndex < activeAudioSources.Count; sourceIndex++)
        {
            AudioSource audioSource = activeAudioSources[sourceIndex];

            if (audioSource.clip == clip)
            {
                return true;
            }
        }

        return false;
    }

    private List<AudioSource> GetAudioClipSources(AudioClip clip)
    {
        List<AudioSource> audioClipSources = new();

        for (int sourceIndex = 0; sourceIndex < activeAudioSources.Count; sourceIndex++)
        {
            AudioSource audioSource = activeAudioSources[sourceIndex];

            if (audioSource.clip == clip)
            {
                audioClipSources.Add(audioSource);
            }
        }

        return audioClipSources;
    }

    public IEnumerator StopSound(AudioClip clip, float delay = default)
    {
        yield return new WaitForSeconds(delay);

        for (int sourceIndex = 0; sourceIndex < activeAudioSources.Count; sourceIndex++)
        {
            AudioSource audioSource = activeAudioSources[sourceIndex];

            if(audioSource.clip == clip)
            {
                activeAudioSources.Remove(audioSource);
                Destroy(audioSource);
            }
        }
    }

    IEnumerator SoundLoop()
    {
        PlaySound(sound, 0.5f);

        yield return new WaitForSeconds(1);

        StartCoroutine(SoundLoop());
    }
}