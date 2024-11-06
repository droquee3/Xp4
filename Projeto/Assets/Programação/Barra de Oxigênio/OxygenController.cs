using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : MonoBehaviour
{
    public Material oxygenMaterial; 
    public float maxOxygen = 1.0f;  
    public float reductionRate = 0.1f; 

    private float currentOxygen; 

    void Start()
    {
        currentOxygen = maxOxygen; 
    }

    void Update()
    {
        currentOxygen -= reductionRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);  

        if (oxygenMaterial != null)
        {
            oxygenMaterial.SetFloat("Reduction", currentOxygen / maxOxygen);
        }
       
    }
}