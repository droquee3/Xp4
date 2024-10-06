using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colidiu com: " + other.gameObject.name);
    }
}