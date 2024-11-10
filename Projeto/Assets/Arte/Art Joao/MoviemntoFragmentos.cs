using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAndRotatingObject : MonoBehaviour
{
    public float amplitude = 0.5f; // Altura do movimento
    public float frequency = 1f; // Velocidade do movimento
    public float rotationSpeed = 50f; // Velocidade de rotação

    private Vector3 startPosition;

    void Start()
    {
        // Salva a posição inicial do objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimento de subida e descida
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);

        // Rotação
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
