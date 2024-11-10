using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButtons : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem particle2;

    private bool donebutton1 = false;
    private bool donebutton2 = false;

    public Transform playerY;

    public Transform button1Y;
    private Vector3 button1OriginalPosition;
    private Vector3 button1PressedPosition;

    public Transform button2Y;
    private Vector3 button2OriginalPosition;
    private Vector3 button2PressedPosition;

    public Transform door;
    private Vector3 doorOriginalPosition;
    private Vector3 doorNewPosition;

    private bool isPressedButton1 = false;
    public bool isPressedButton2 = false;

    public GameObject freeze;  
    public GameObject freezeR; 

    void Start()
    {
        isPressedButton1 = false;
        isPressedButton2 = false;

        doorOriginalPosition = door.position;
        doorNewPosition = doorOriginalPosition + new Vector3(3.7f, 0, -2.9f);

        // Define as posições inicial e pressionada do botão 1
        button1OriginalPosition = button1Y.position;
        button1PressedPosition = button1OriginalPosition + new Vector3(0, -0.4f, 0);

        // Define as posições inicial e pressionada do botão 2
        button2OriginalPosition = button2Y.position;
        button2PressedPosition = button2OriginalPosition + new Vector3(0, -0.4f, 0);
        
        // Inicializa o estado dos objetos freeze e freezeR
        if (freeze != null && freezeR != null)
        {
            freeze.SetActive(true);   
            freezeR.SetActive(false); 
        }
    }

    void Update()
    {
        if (isPressedButton1 && !donebutton1)
        {
            particle.Play();
            donebutton1 = true;
        }
        if (isPressedButton2 && !donebutton2)
        {
            particle2.Play();
            donebutton2 = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button1") && playerY.position.y > button1Y.position.y && !isPressedButton1)
        {
            isPressedButton1 = true;
            Debug.Log("[Collision] Colidiu com button1");
            StartCoroutine(PressedButton()); // Inicia a animação
        }

        if (collision.gameObject.CompareTag("Button2") && playerY.position.y > button2Y.position.y && !isPressedButton2)
        {
            isPressedButton2 = true;
            Debug.Log("[Collision] Colidiu com button2");
            StartCoroutine(PressedButton()); // Inicia a animação
        }
    }

    private IEnumerator PressedButton()
    {
        if (isPressedButton2)
        {
            float elapsedTime = 0;
            float animationDuration = 0.5f;
            float dooranimationDuration = 2.0f;

            // Lógica para pressionar o botão 2
            while (elapsedTime < animationDuration)
            {
                button2Y.position = Vector3.Lerp(button2OriginalPosition, button2PressedPosition, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            button2Y.position = button2PressedPosition;

            // Lógica para mover a porta
            while (elapsedTime < dooranimationDuration)
            {
                door.position = Vector3.Lerp(doorOriginalPosition, doorNewPosition, elapsedTime / dooranimationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            door.position = doorNewPosition;
        }

        if (isPressedButton1)
        {
            float elapsedTime = 0;
            float animationDuration = 0.5f;

            while (elapsedTime < animationDuration)
            {
                button1Y.position = Vector3.Lerp(button1OriginalPosition, button1PressedPosition, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            button1Y.position = button1PressedPosition;

            if (freeze != null && freezeR != null)
            {
                freeze.SetActive(false);  // Torna o objeto freeze invisível
                freezeR.SetActive(true);  // Torna o objeto freezeR visível
            }
        }
    }
}
