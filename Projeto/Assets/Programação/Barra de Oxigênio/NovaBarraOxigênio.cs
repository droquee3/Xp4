using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaBarraOxigênio : MonoBehaviour
{
    public Material oxygenMaterial; // Material que será alterado
    public float maxOffset = 0.5f; // Valor máximo do offset
    public float minOffset = -0.5f; // Valor mínimo do offset
    public float reductionRate = 0.1f; // Taxa de redução (quanto será reduzido por segundo)

    private float currentOffset;

    void Start()
    {
        currentOffset = maxOffset; // Começa com o offset no valor máximo
        UpdateMaterial(); // Inicializa o material com o valor correto
    }

    void Update()
    {
        if (currentOffset > minOffset)
        {
            // Reduz o offset de forma contínua ao longo do tempo
            currentOffset -= reductionRate * Time.deltaTime;
            currentOffset = Mathf.Clamp(currentOffset, minOffset, maxOffset);

            // Atualiza o material para refletir o offset atual
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