using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhoGigante : MonoBehaviour
{
    public GameObject eye;  
    public Vector3 offset = new Vector3(0, 5, 10);  

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("TriggerZone"))
        {
            
            Vector3 eyePosition = transform.position + transform.forward * offset.z + transform.up * offset.y;
            eye.transform.position = eyePosition;

         
            eye.SetActive(true);
        }
    }
}