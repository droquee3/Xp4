using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraOxigenio2 : MonoBehaviour
{
    public Material oxygenMaterial;
    public Color fullOxygenColor = Color.blue;
    public Color emptyOxygenColor = Color.red;
    public float maxHeight = 1f;
    public float decreaseRate = 0.1f;
    public GameObject player;

    private float currentHeight;
    private bool isOxygenDepleted = false;

    void Start()
    {
        currentHeight = maxHeight;
        UpdateOxygenColor();
        UpdateOxygenFill();
    }

    void Update()
    {
        if (!isOxygenDepleted)
        {
            currentHeight -= decreaseRate * Time.deltaTime;
            currentHeight = Mathf.Clamp(currentHeight, 0, maxHeight);

            UpdateOxygenColor();
            UpdateOxygenFill();

            if (currentHeight <= 0)
            {
                isOxygenDepleted = true;
            }
        }
    }

    void UpdateOxygenColor()
    {
        float oxygenLevel = currentHeight / maxHeight;
        Color currentColor = Color.Lerp(emptyOxygenColor, fullOxygenColor, oxygenLevel);
        oxygenMaterial.SetColor("_BaseColor", currentColor);
    }
    void UpdateOxygenFill()
    {
        float fillAmount = currentHeight / maxHeight;
        oxygenMaterial.SetFloat("_FillAmount", fillAmount); // "_FillAmount" deve ser uma propriedade exposta no shader
    }
}