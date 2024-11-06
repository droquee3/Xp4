using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraOxigenio2 : MonoBehaviour
{
    public Material oxygenMaterial; 
    public Color fullOxygenColor = Color.blue;
    public Color emptyOxygenColor = Color.red; 
    public float decreaseRate = 0.1f; 

    private float currentOxygen;
    private float maxOxygen = 1f; 
    private bool isOxygenDepleted = false;

    void Start()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenColor();
    }

    void Update()
    {
        if (!isOxygenDepleted)
        {
           
            currentOxygen -= decreaseRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

            UpdateOxygenColor();

            if (currentOxygen <= 0)
            {
                isOxygenDepleted = true;
            }
        }
    }

    void UpdateOxygenColor()
    {
        
        float oxygenLevel = currentOxygen / maxOxygen;

       
        Color currentColor = Color.Lerp(emptyOxygenColor, fullOxygenColor, oxygenLevel);

     
        oxygenMaterial.SetColor("_BaseColor", currentColor);
    }
}
