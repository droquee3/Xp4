using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColetorMemoria : MonoBehaviour
{
    public GameObject particulas; 
    public float tempoAntesDeCarregarNivel = 2f;  

    private bool colidiu = false; 

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !colidiu)
        {
            colidiu = true;  

            if (particulas != null)
            {
                particulas.transform.position = transform.position;
                particulas.SetActive(true);
            }

            gameObject.SetActive(false);

  
            Invoke("CarregarMenuPrincipal", tempoAntesDeCarregarNivel);
        }
    }

    void CarregarMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}