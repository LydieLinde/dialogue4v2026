using System.Collections.Generic;
using UnityEngine;

// Gerencia reprodução global de áudio (singleton).
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // AudioSource usado para músicas/sistema
    private AudioSource _systemSource;
    private List<AudioSource> _activeSources;

    [Header("Collection (Music)")]
    [Tooltip("Lista de músicas que podem ser reproduzidas via AudioManager")]
    public List<AudioClip> audioCollection = new List<AudioClip>();

    [Tooltip("Índice atual na coleção")]
    public int collectionIndex;

    [Tooltip("Se verdadeiro, a música de sistema ficará em loop")]
    public bool loopMusic = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // garante que exista um AudioSource no GameObject
            _systemSource = gameObject.GetComponent<AudioSource>();
            if (_systemSource == null)
            {
                _systemSource = gameObject.AddComponent<AudioSource>();
            }

            _systemSource.loop = loopMusic;
            _activeSources = new List<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        // Mantém index válido no editor
        if (audioCollection != null && audioCollection.Count > 0)
        {
            collectionIndex = Mathf.Clamp(collectionIndex, 0, audioCollection.Count - 1);
        }
        else
        {
            collectionIndex = 0;
        }
    }

    // ---------- Funções de gerenciamento de audio (música/sistema) ----------
    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        if (_systemSource == null) return;

        _systemSource.loop = loopMusic;
        _systemSource.Stop();
        _systemSource.clip = clip;
        _systemSource.Play();
    }

    public void StopSound()
    {
        if (_systemSource == null) return;
        _systemSource.Stop();
    }

    public void PauseSound()
    {
        if (_systemSource == null) return;
        _systemSource.Pause();
    }

    public void ResumeSound()
    {
        if (_systemSource == null) return;
        _systemSource.UnPause();
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip == null || _systemSource == null) return;
        _systemSource.PlayOneShot(clip);
    }

    // ---------- Coleção: Play por índice, set/get ----------
    public void PlayCollectionIndex(int index)
    {
        if (audioCollection == null || audioCollection.Count == 0) return;
        if (index < 0 || index >= audioCollection.Count) return;
        collectionIndex = index;
        PlaySound(audioCollection[collectionIndex]);
    }

    public void PlayCurrentCollection()
    {
        PlayCollectionIndex(collectionIndex);
    }

    public void SetCollectionIndex(int index)
    {
        if (audioCollection == null || audioCollection.Count == 0) return;
        collectionIndex = Mathf.Clamp(index, 0, audioCollection.Count - 1);
    }

    public AudioClip GetCurrentClip()
    {
        if (audioCollection == null || audioCollection.Count == 0) return null;
        if (collectionIndex < 0 || collectionIndex >= audioCollection.Count) return null;
        return audioCollection[collectionIndex];
    }

    // ---------- Funções de gerenciamento de fontes de audio 3D ----------
    public void PlaySound(AudioClip clip, AudioSource source)
    {
        if (source == null || clip == null) return;
        if (!_activeSources.Contains(source)) _activeSources.Add(source);
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void StopSound(AudioSource source)
    {
        if (source == null) return;
        source.Stop();
        _activeSources.Remove(source);
    }

    public void PauseSound(AudioSource source)
    {
        if (source == null) return;
        source.Pause();
    }

    public void ResumeSound(AudioSource source)
    {
        if (source == null) return;
        source.UnPause();
    }

    public void PlayOneShot(AudioClip clip, AudioSource source)
    {
        if (source == null || clip == null) return;
        source.PlayOneShot(clip);
    }
}

