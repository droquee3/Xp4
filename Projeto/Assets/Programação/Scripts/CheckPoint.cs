using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public NovaBarraOxig�nio oxygenBar;
    public float minOffset = -0.5f; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Trigger colidido!"); 

            if (oxygenBar != null)
            {
                oxygenBar.SetOxygenToMinOffset(minOffset);
            }
        }
    }
}