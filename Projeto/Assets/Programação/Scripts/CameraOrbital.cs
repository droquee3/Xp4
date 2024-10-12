using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbital : MonoBehaviour
{
    public Transform target;      
    public float distance = 5.0f; 
    public float xSpeed = 150.0f; 
    public float ySpeed = 150.0f; 

    public float yMinLimit = 1f;  
    public float yMaxLimit = 80f; 
    public float minHeight = 1.0f;
    
    public float autoRotateSpeed = 5.0f; 

    private float x = 0.0f; 
    private float y = 0.0f; 

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (Time.timeScale > 0)
        {
            float mouseX = Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            float mouseY = Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            float rightStickX = Input.GetAxis("Right Stick X") * xSpeed * 0.1f;
            float rightStickY = Input.GetAxis("Right Stick Y") * ySpeed * 0.1f;

            x += mouseX + rightStickX;
            y -= mouseY + rightStickY;
        }
        else
        {
            x -= autoRotateSpeed * Time.unscaledDeltaTime; 
        }

        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
