using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlinharCamera : MonoBehaviour
{
    public Transform cameraTransform; 
    public float rotationSpeed = 5f;  
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  
        rb.freezeRotation = true;        
    }

    void Update()
    {
       
        Vector3 directionToCamera = cameraTransform.forward;
        directionToCamera.y = 0f; 

     
        if (directionToCamera.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}