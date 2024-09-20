using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    [SerializeField] GameObject player;
    
    [SerializeField] List<GameObject> checkPoints;
    
    [SerializeField] Vector3 vectorPoint;

    [SerializeField] bool dead = false;

    void Update()
    {
        // Verifica se a tecla Q foi pressionada
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dead = true;
        }

        if (dead == true) {
            player.transform.position = vectorPoint;    
            dead = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        vectorPoint = player.transform.position;
        Destroy(other.gameObject);
    }
}
