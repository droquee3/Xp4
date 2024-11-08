using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButtons : MonoBehaviour {

    public Transform playerY;
    public Transform button1Y;
    public Transform button2Y;
    public Transform door; // Adiciona uma referência à porta
    public Transform targetPosition; // A posição para onde a porta deve se mover

    private bool isPressedButton1 = false;
    private bool isPressedButton2 = false;

    void Start() {
        isPressedButton1 = false;
        isPressedButton2 = false;
    }

    void Update() {
        // Verifica se a porta deve ser movida
        if (isPressedButton2) {
            MoveDoor();
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

    // Função para mover a porta
    private void MoveDoor() {
        // Mover a porta para a posição do outro objeto
        door.position = targetPosition.position;
        //Debug.Log("[Movement] Porta se moveu para a posição do target.");
    }
}
