using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomBatimentos : MonoBehaviour
{
    public static SomBatimentos Instance;
    public AudioClip heartbeatSound; // Som de batimento cardíaco
    private GameObject audioObject; // Objeto temporário para tocar o som

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
            // Cria um objeto temporário para tocar o som em loop
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
            Destroy(audioObject); // Destrói o objeto de áudio quando o som deve parar
            audioObject = null;
        }
    }
}
