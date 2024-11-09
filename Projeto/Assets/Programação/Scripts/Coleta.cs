using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
  
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EsferaColetavel"))
        {
            other.gameObject.SetActive(false);
        }
    }
}