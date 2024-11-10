using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
    private AudioSource fragmentSound;

    void Start()
    {
        fragmentSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EsferaColetavel"))
        {
            other.gameObject.SetActive(false);

            if (fragmentSound != null && !fragmentSound.isPlaying)
            {
                fragmentSound.Play(); 
            }
        }
    }
}