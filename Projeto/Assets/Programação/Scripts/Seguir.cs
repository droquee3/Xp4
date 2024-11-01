using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seguir : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float rotationSmoothness = 0.1f;
    public Transform cameraTransform;

    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("A câmera não foi definida!");
        }
    }

    void Update()
    {

        transform.position = player.position + offset;

        if (cameraTransform != null)
        {
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            directionToCamera.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness);
        }
    }
}