using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour
{
    public float pushForce = 10f;
    public float delayBeforePush = 2f;
    public float slowdownFactor = 0.5f;
    private Rigidbody playerRb;
    private bool isPushing = false;
    private float timeInside = 0f;
    private UnderwaterMovement playerMovementScript;
    private Vector3 pushDirection;
    private Vector3 lastPlayerVelocity; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            playerRb = other.GetComponent<Rigidbody>();
            playerMovementScript = other.GetComponent<UnderwaterMovement>();

          
            timeInside = 0f;
            isPushing = false;

           
            lastPlayerVelocity = playerRb.velocity;

            
            pushDirection = -lastPlayerVelocity.normalized;
            pushDirection.y = 0; 
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerRb != null && playerMovementScript != null)
        {
            
            timeInside += Time.deltaTime;

            
            if (timeInside >= delayBeforePush)
            {
                if (!isPushing)
                {
                    
                    playerMovementScript.enabled = false;
                    isPushing = true;
                }

                
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Acceleration);
            }
            else
            {
                
                if (playerRb.velocity.magnitude > 0)
                {
                    playerRb.velocity *= slowdownFactor;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            isPushing = false;
            timeInside = 0f;

            
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true;
            }

           
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;
            }

            playerRb = null;
            playerMovementScript = null;
        }
    }
}
