using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButtons : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem particle2;
    [SerializeField] AudioSource button1Audio;
    [SerializeField] AudioSource button2Audio;

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

     
        button1OriginalPosition = button1Y.position;
        button1PressedPosition = button1OriginalPosition + new Vector3(0, -0.4f, 0);

        button2OriginalPosition = button2Y.position;
        button2PressedPosition = button2OriginalPosition + new Vector3(0, -0.4f, 0);

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
            button1Audio.Play();
            donebutton1 = true;

        }
        if (isPressedButton2 && !donebutton2)
        {
            particle2.Play();
            button2Audio.Play();
            donebutton2 = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button1") && playerY.position.y > button1Y.position.y && !isPressedButton1)
        {
            isPressedButton1 = true;
            Debug.Log("[Collision] Colidiu com button1");
            StartCoroutine(PressedButton()); 
        }

        if (collision.gameObject.CompareTag("Button2") && playerY.position.y > button2Y.position.y && !isPressedButton2)
        {
            isPressedButton2 = true;
            Debug.Log("[Collision] Colidiu com button2");
            StartCoroutine(PressedButton()); 
        }
    }

    private IEnumerator PressedButton()
    {
        if (isPressedButton2)
        {
            float elapsedTime = 0;
            float animationDuration = 0.5f;
            float dooranimationDuration = 2.0f;

        
            while (elapsedTime < animationDuration)
            {
                button2Y.position = Vector3.Lerp(button2OriginalPosition, button2PressedPosition, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            button2Y.position = button2PressedPosition;

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
                freeze.SetActive(false);  
                freezeR.SetActive(true);  
            }
        }
    }
}