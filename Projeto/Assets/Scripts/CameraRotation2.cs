using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation2 : MonoBehaviour
{
    public float rotationSpeed = 5f;             
    public Vector2 verticalLimits = new Vector2(-30, 60); 
    public CinemachineVirtualCamera virtualCamera; 
    public float smoothTime = 0.1f;             

    private float currentYaw = 0f;                
    private float currentPitch = 0f;              
    private float yawSmoothVelocity;              
    private float pitchSmoothVelocity;            
    private float targetYaw = 0f;                 
    private float targetPitch = 0f;              

    void Start() { }
    

    void Update()
    {
      
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

      
        targetYaw += mouseX;
        targetPitch -= mouseY; 
        targetPitch = Mathf.Clamp(targetPitch, verticalLimits.x, verticalLimits.y); 

       
        currentYaw = Mathf.SmoothDamp(currentYaw, targetYaw, ref yawSmoothVelocity, smoothTime);
        currentPitch = Mathf.SmoothDamp(currentPitch, targetPitch, ref pitchSmoothVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, currentYaw, 0);

        
        if (virtualCamera != null)
        {
            var cameraTransform = virtualCamera.transform;
            cameraTransform.localRotation = Quaternion.Euler(currentPitch, 0, 0);
        }
    }
}