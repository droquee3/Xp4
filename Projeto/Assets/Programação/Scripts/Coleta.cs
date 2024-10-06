using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
    public GameObject esferaPrincipal;  
    public int totalEsferas = 3;        
    private int esferasColetadas = 0;   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EsferaColetavel"))
        {
            other.gameObject.SetActive(false);

            esferasColetadas++;

            if (esferasColetadas >= totalEsferas)
            {
                if (esferaPrincipal != null)
                {
                    esferaPrincipal.SetActive(true);
                }
            }
        }
    }
}