using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomBatimentos : MonoBehaviour
{
    public static SomBatimentos Instance;
    public AudioClip heartbeatSound; // Som de batimento card�aco
    private GameObject audioObject; // Objeto tempor�rio para tocar o som

    void Awake()
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

    public void PlayHeartbeatSound()
    {
        if (heartbeatSound != null && audioObject == null)
        {
            // Cria um objeto tempor�rio para tocar o som em loop
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
            Destroy(audioObject); // Destr�i o objeto de �udio quando o som deve parar
            audioObject = null;
        }
    }
}
