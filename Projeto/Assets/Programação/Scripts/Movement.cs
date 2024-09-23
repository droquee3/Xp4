using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public float acceleration = 10f;  
    public float maxSpeed = 5f;       
    public float drag = 5f;           

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag; 
    }

    private void FixedUpdate()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");     

        
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

       
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movement * acceleration, ForceMode.Acceleration);
        }

        
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
}