using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesativadorDeTextos : MonoBehaviour
{
    public TMP_Text textoParaDesativar; 
    public float tempoParaDesativar = 5f;  

    void Start()
    {
        
        StartCoroutine(DesativarAposTempo());
    }

    IEnumerator DesativarAposTempo()
    {
       
        yield return new WaitForSeconds(tempoParaDesativar);
        textoParaDesativar.gameObject.SetActive(false);
    }
}
