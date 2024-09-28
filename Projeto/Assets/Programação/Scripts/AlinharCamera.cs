using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlinharCamera : MonoBehaviour
{
    public Transform cameraTransform;  // Refer�ncia � c�mera
    public float rotationSpeed = 5f;   // Velocidade de rota��o do personagem
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Obt�m o Rigidbody do personagem
        rb.freezeRotation = true;        // Travar a rota��o do Rigidbody
    }

    void Update()
    {
        // Calcular a dire��o em que a c�mera est� olhando, ignorando a componente Y
        Vector3 directionToCamera = cameraTransform.forward;
        directionToCamera.y = 0f;  // Zera a componente Y para manter o personagem "no ch�o"

        // Se a dire��o for maior que um valor muito pequeno, aplicar a rota��o
        if (directionToCamera.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}