using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public NovaBarraOxig�nio oxygenBar;
    public float minOffset = -0.5f;
    private AudioSource respiracao;

    void Start()
    {
        respiracao = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Trigger colidido!");

            if (oxygenBar != null)
            {
                oxygenBar.SetOxygenToMinOffset(minOffset);
                oxygenBar.SetCheckpointPosition(transform.position);
                oxygenBar.RestartOxygenDelay(); // Chama o m�todo para reiniciar o atraso
                respiracao.Play();
            }
        }
    }
}