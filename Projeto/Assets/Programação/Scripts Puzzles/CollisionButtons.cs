using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButtons : MonoBehaviour
{

    public Transform playerY;
    public Transform button1Y;
    public Transform button2Y;
    public Transform door;
    public Transform targetPosition;

    private bool isPressedButton1 = false;
    private bool isPressedButton2 = false;
    private Vector3 button2OriginalPosition;
    private Vector3 button2PressedPosition;

    void Start()
    {
        isPressedButton1 = false;
        isPressedButton2 = false;

        // Define as posições inicial e pressionada do botão 2
        button2OriginalPosition = button2Y.position;
        button2PressedPosition = button2OriginalPosition + new Vector3(0, -0.4f, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button1") && playerY.position.y > button1Y.position.y)
        {
            isPressedButton1 = true;
            Debug.Log("[Collision] Colidiu com button1");
        }

        if (collision.gameObject.CompareTag("Button2") && playerY.position.y > button2Y.position.y && !isPressedButton2)
        {
            isPressedButton2 = true;
            Debug.Log("[Collision] Colidiu com button2");
            StartCoroutine(OpenDoorAndAnimateButton()); // Inicia a animação
        }
    }

    private IEnumerator OpenDoorAndAnimateButton()
    {
        // Animação de descida do botão
        float elapsedTime = 0;
        float animationDuration = 0.5f;

        while (elapsedTime < animationDuration)
        {
            button2Y.position = Vector3.Lerp(button2OriginalPosition, button2PressedPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        button2Y.position = button2PressedPosition;

        // Mover a porta após o botão descer
        door.position = targetPosition.position;
        Debug.Log("[Movement] Porta movida para a posição alvo.");
    }
}


