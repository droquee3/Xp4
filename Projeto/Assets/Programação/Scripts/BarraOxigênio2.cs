using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraOxigênio2 : MonoBehaviour
{

    public Material oxygenMaterial; // Associe o material da barra
    public Color fullOxygenColor = Color.blue; // Cor para oxigênio cheio
    public Color emptyOxygenColor = Color.red; // Cor para oxigênio vazio
    public float maxHeight = 1f;
    public float decreaseRate = 0.1f;
    public GameObject player;

    private float currentHeight;
    private bool isOxygenDepleted = false;

    void Start()
    {
        currentHeight = maxHeight;
        UpdateOxygenColor();
    }

    void Update()
    {
        if (!isOxygenDepleted)
        {
            currentHeight -= decreaseRate * Time.deltaTime;
            currentHeight = Mathf.Clamp(currentHeight, 0, maxHeight);

            UpdateOxygenColor();

            if (currentHeight <= 0)
            {
                isOxygenDepleted = true;
            }
        }
    }

    void UpdateOxygenColor()
    {
        float oxygenLevel = currentHeight / maxHeight; // Valor entre 0 e 1
        Color currentColor = Color.Lerp(emptyOxygenColor, fullOxygenColor, oxygenLevel);
        oxygenMaterial.SetColor("_BaseColor", currentColor); // Ajuste para o shader padrão ou "_OxygenColor" para Shader Graph
    }
}
