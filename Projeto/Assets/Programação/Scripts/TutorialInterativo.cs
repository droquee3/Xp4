using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialInterativo : MonoBehaviour
{
    public GameObject movimentoTexto;  
    public GameObject puloTexto;       

    private bool movimentoCompletado = false;
    private bool puloCompletado = false;

    void Update()
    {
       
        if (!movimentoCompletado && VerificarMovimento())
        {
            movimentoCompletado = true;
            movimentoTexto.SetActive(false);
            puloTexto.SetActive(true);       
        }
        if (movimentoCompletado && !puloCompletado && VerificarPulo())
        {
            puloCompletado = true;
            puloTexto.SetActive(false);      
        }
    }

    bool VerificarMovimento()
    {
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
               Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
               Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    bool VerificarPulo()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump");
    }
}