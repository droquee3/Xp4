using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButtons : MonoBehaviour {

    public GameObject[] iceSpikes;

    public Transform playerY;
    public Transform button1Y;
    public Transform button2Y;
    public Transform objectToMove; // Objeto que será movido
    public float targetXPosition = -395.16f; // Posição x alvo do objeto
    public float moveSpeed = 2f; // Velocidade do movimento

    private bool isPressedButton1 = false;
    private bool isPressedButton2 = false;

    void Start() {
        isPressedButton1 = false;
        isPressedButton2 = false;
    }

    void Update() {
        if (isPressedButton1 || isPressedButton2) {
            MoveObjectToTarget();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Button1") && playerY.position.y > button1Y.position.y) {
            isPressedButton1 = true;
            Debug.Log("[Collision] Colidiu com button1");
        }

        if (collision.gameObject.CompareTag("Button2") && playerY.position.y > button2Y.position.y) {
            isPressedButton2 = true;
            Debug.Log("[Collision] Colidiu com button2");
        }
    }

    void MoveObjectToTarget() {
        Vector3 currentPosition = objectToMove.position;
        Vector3 targetPosition = new Vector3(targetXPosition, currentPosition.y, currentPosition.z);

        // Move o objeto gradualmente para a posição alvo no eixo X
        objectToMove.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Verifica se o objeto chegou na posição alvo
        if (objectToMove.position.x == targetXPosition) {
            isPressedButton1 = false;
            isPressedButton2 = false;
        }
    }
}
