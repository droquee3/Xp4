using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteTexto : MonoBehaviour
{
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;  
    }

    void LateUpdate()
    {
        transform.rotation = originalRotation;  
    }
}