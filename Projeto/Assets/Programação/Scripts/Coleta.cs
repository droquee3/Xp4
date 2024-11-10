using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
    [SerializeField] ParticleSystem particle1;
    [SerializeField] ParticleSystem particle2;
    [SerializeField] ParticleSystem particle3;

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
                particle1.Play();
                particle2.Play();
                particle3.Play();
            }
        }
    }
}