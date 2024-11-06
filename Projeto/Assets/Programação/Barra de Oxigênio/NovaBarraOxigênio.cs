using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaBarraOxigênio : MonoBehaviour
{
    public Material oxygenMaterial; 
    public float maxOffset = 0.5f; 
    public float minOffset = -0.5f; 
    public float reductionRate = 0.1f; 

    private float currentOffset;

    void Start()
    {
        currentOffset = maxOffset; 
        UpdateMaterial(); 
    }

    void Update()
    {
        if (currentOffset > minOffset)
        {
            currentOffset -= reductionRate * Time.deltaTime;
            currentOffset = Mathf.Clamp(currentOffset, minOffset, maxOffset);
            UpdateMaterial();
        }
    }

    void UpdateMaterial()
    {
        if (oxygenMaterial != null)
        {
            oxygenMaterial.SetFloat("_offset", currentOffset);
        }
        else
        {
            Debug.LogWarning("Oxygen Material not assigned.");
        }
    }
}