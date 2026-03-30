using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
   
    
    public AudioCollection myAudioCollection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        AudioManager.Instance.PlaySound(myAudioCollection.AudioClipCollection[0]);
    }
        
}

