using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : MonoBehaviour
{
    public Material oxygenMaterial;
    public float maxOxygen = 1.0f;
    public float reductionRate = 0.1f;
    public float reductionInterval = 1.0f;

    private float currentOxygen;
    private float reductionTimer;

    void Start()
    {
        currentOxygen = maxOxygen;
        reductionTimer = reductionInterval;
    }

    void Update()
    {
        reductionTimer -= Time.deltaTime;

        if (reductionTimer <= 0)
        {
            currentOxygen -= reductionRate;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
            reductionTimer = reductionInterval;

            if (oxygenMaterial != null)
            {
                oxygenMaterial.SetFloat("_FillAmount", currentOxygen / maxOxygen);
                Debug.Log("Current Oxygen Level: " + currentOxygen / maxOxygen);
            }
            else
            {
                Debug.LogWarning("Oxygen Material not assigned.");
            }
        }
    }
}