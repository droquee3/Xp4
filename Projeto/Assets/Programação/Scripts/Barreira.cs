using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour
{
    public float pushForce = 10f;
    public float delayBeforePush = 2f;
    public float slowdownFactor = 0.5f;
    private Rigidbody playerRb;
    private bool isPushing = false;
    private float timeInside = 0f;
    private UnderwaterMovement playerMovementScript;
    private Vector3 pushDirection;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtém Rigidbody e script de movimento do jogador
            playerRb = other.GetComponent<Rigidbody>();
            playerMovementScript = other.GetComponent<UnderwaterMovement>();

            // Reseta o tempo e o estado de empurrão
            timeInside = 0f;
            isPushing = false;

            // Calcula a direção do empurrão com base na posição relativa do jogador e da barreira
            pushDirection = (other.transform.position - transform.position).normalized;
            pushDirection.y = 0;  // Ignora a componente Y para garantir que o empurrão seja no plano XZ
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerRb != null && playerMovementScript != null)
        {
            // Incrementa o tempo dentro da barreira
            timeInside += Time.deltaTime;

            // Após o atraso, começa a empurrar o jogador
            if (timeInside >= delayBeforePush)
            {
                if (!isPushing)
                {
                    // Desativa o movimento do jogador enquanto ele é empurrado
                    playerMovementScript.enabled = false;
                    isPushing = true;
                }

                // Aplica força na direção calculada
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Acceleration);
            }
            else
            {
                // Desacelera o jogador se ele tentar se mover antes do empurrão
                if (playerRb.velocity.magnitude > 0)
                {
                    playerRb.velocity *= slowdownFactor;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reseta as variáveis ao sair da barreira
            isPushing = false;
            timeInside = 0f;

            // Reativa o movimento do jogador e reseta a velocidade do Rigidbody
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true;
            }

            // Zera a velocidade para evitar força residual
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;
            }

            playerRb = null;
            playerMovementScript = null;
        }
    }
}
