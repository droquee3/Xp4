using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f; // Altura do movimento
    public float frequency = 1f; // Velocidade do movimento

    private Vector3 startPosition;

    void Start()
    {
        // Salva a posição inicial do objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcula o movimento de subida e descida
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        // Aplica o movimento ao objeto
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}

