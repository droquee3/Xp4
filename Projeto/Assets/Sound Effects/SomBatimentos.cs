using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomBatimentos : MonoBehaviour
{
    public static SomBatimentos Instance;
    public AudioClip heartbeatSound; 
    private GameObject audioObject; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHeartbeatSound()
    {
        if (heartbeatSound != null && audioObject == null)
        {
            audioObject = new GameObject("HeartbeatAudio");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = heartbeatSound;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.Play();
        }
    }

    public void StopHeartbeatSound()
    {
        if (audioObject != null)
        {
            Destroy(audioObject); 
            audioObject = null;
        }
    }
}
