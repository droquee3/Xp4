using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
    [SerializeField] ParticleSystem particle1;
    [SerializeField] ParticleSystem particle2;
    [SerializeField] ParticleSystem particle3;

    private AudioSource fragmentSound;

    public GameObject picBanheiro; 
    public GameObject picParkour; 
    public GameObject picGeladeira;


    void Start()
    {
        fragmentSound = GetComponent<AudioSource>();
        picBanheiro.SetActive(false);
        picParkour.SetActive(false);
        picGeladeira.SetActive(false);
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
            if (other.gameObject.name == "Cylinder.006")
            {
                Invoke("ShowPicGeladeira", 1f);
            }
            if (other.gameObject.name == "Fragmento Espelho")
            {
                Invoke("ShowPicBanheiro", 1f);
            }
            if (other.gameObject.name == "Cylinder.007")
            {
                Invoke("ShowPicParkour", 1f);
            }
        }
        
    }

    void ShowPicBanheiro()
    {
        picBanheiro.SetActive(true);
        Invoke("HidePic", 4f);
    }
    void ShowPicGeladeira()
    {
        picGeladeira.SetActive(true);

        Invoke("HidePic", 4f);
    }
    void ShowPicParkour()
    {
        picParkour.SetActive(true);

        Invoke("HidePic", 4f);
    }

    void HidePic()
    {
        picBanheiro.SetActive(false);
        picParkour.SetActive(false);
        picGeladeira.SetActive(false);
    }
}