using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlinharCamera : MonoBehaviour
{
    public Transform cameraTransform;  // Referência à câmera
    public float rotationSpeed = 5f;   // Velocidade de rotação do personagem
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Obtém o Rigidbody do personagem
        rb.freezeRotation = true;        // Travar a rotação do Rigidbody
    }

    void Update()
    {
        // Calcular a direção em que a câmera está olhando, ignorando a componente Y
        Vector3 directionToCamera = cameraTransform.forward;
        directionToCamera.y = 0f;  // Zera a componente Y para manter o personagem "no chão"

        // Se a direção for maior que um valor muito pequeno, aplicar a rotação
        if (directionToCamera.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}