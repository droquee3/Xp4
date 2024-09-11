using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation2 : MonoBehaviour
{
    public float horizontalSpeed = 5f; 
    public float verticalSpeed = 5f;   
    public float verticalClamp = 80f; 

    private float verticalRotation = 0f; 

    void Update()
    {
       
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

      
        float horizontalRotation = horizontalInput * horizontalSpeed;
        verticalRotation -= verticalInput * verticalSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);

       
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localRotation.eulerAngles.y + horizontalRotation, 0f);
    }
}
