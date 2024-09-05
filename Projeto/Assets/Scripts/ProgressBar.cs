using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float maxHeight = 1f;          
    public float decreaseRate = 0.1f;      
    public float delayBeforeStart = 2f;    
    public float resetDelay = 3f;          

    private float currentHeight;
    private float elapsedTime = 0f;         
    private float resetElapsedTime = 0f;    
    private bool isResetting = false;       

    void Start()
    {
        currentHeight = maxHeight;
        UpdateScale();
    }

    void Update()
    {
        if (isResetting)
        {
            resetElapsedTime += Time.deltaTime;

            if (resetElapsedTime >= resetDelay)
            {
                isResetting = false;
                elapsedTime = 0f; 
                UpdateScale();
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= delayBeforeStart)
            {
                currentHeight -= decreaseRate * Time.deltaTime;
                currentHeight = Mathf.Clamp(currentHeight, 0, maxHeight);

                UpdateScale();

                if (currentHeight <= 0)
                {
                    enabled = false; 
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ResetObject")) 
        {
            currentHeight = maxHeight;
            UpdateScale();
            isResetting = true;
            resetElapsedTime = 0f;
        }
    }

    void UpdateScale()
    {
        transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
        transform.localPosition = new Vector3(transform.localPosition.x, maxHeight / 2f - currentHeight / 2f, transform.localPosition.z);
    }
}