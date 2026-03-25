using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    private AudioSource systemSource;
    private List<AudioSource> activeSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //Funções de gerenciamento de audio
    public void PlaySound(AudioClip clip)
    {
      systemSource.Stop();
      systemSource.clip = clip;
      systemSource.Play();
    }

    public void StopSound()
    {
      systemSource.Stop();
    }

    public void PauseSound()
    {
        systemSource.Pause();
    }

    public void ResumeSound()
    {
        systemSource.UnPause();
    }
    
    public void PlayOneShot(AudioClip clip)
    {
      systemSource.PlayOneShot(clip);
    }
    
    //Funções de gerenciamento de fontes de audio 3d
    public void PlaySound(AudioClip clip, AudioSource source)
    {
    if(!activeSources.Contains(source)) activeSources.Add(source);
    source.Stop();
    source.clip = clip;
    source.Play();
    
    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
        activeSources.Remove(source);
    }

    public void PauseSound(AudioSource source)
    {
        source.Pause();
    }

    public void ResumeSound(AudioSource source)
    {
        source.UnPause();
    }

    public void PlayOneShot(AudioClip clip, AudioSource source)
    {
        source.PlayOneShot(clip);
    }
}

