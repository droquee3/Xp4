using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhoGigante : MonoBehaviour
{
    public GameObject eye;
    public Vector3 offset = new Vector3(0, 5, 10);  
    public Transform player;  
    private bool eyeActivated = false;  

    void Start()
    {
        
        if (eye != null)
        {
            eye.SetActive(false);  
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !eyeActivated)
        {
           
            Vector3 eyePosition = transform.position + transform.forward * offset.z + transform.up * offset.y;
            eye.transform.position = eyePosition;

            
            eye.SetActive(true);

                LookAtPlayer();

            
            eyeActivated = true;
        }
    }

    void Update()
    {
        
        if (eyeActivated && player != null)
        {
            
            LookAtPlayer();
        }
    }

   
    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - eye.transform.position;
    
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        
        targetRotation *= Quaternion.Euler(90, 0, 0); 
        eye.transform.rotation = targetRotation;
    }
}