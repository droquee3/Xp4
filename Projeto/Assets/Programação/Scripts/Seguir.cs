using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seguir : MonoBehaviour
{
    public Transform player;  
    public Vector3 offset;   

    void Update()
    {
      
        transform.position = player.position + offset;

       
        transform.rotation = Quaternion.identity;

       
        Vector3 localPosition = transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
    }
}