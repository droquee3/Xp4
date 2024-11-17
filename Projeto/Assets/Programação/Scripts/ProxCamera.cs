using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxCamera : MonoBehaviour
{
    public CameraOrbital cameraOrbital;
    public float zoomOutDistance = 10f;
    public float smoothTime = 0.5f;

    private float originalDistance;
    private Coroutine zoomCoroutine;

    void Start()
    {
        if (cameraOrbital != null)
        {
            originalDistance = cameraOrbital.distance;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cameraOrbital != null)
        {
            if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(ZoomToDistance(zoomOutDistance));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && cameraOrbital != null)
        {
            if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(ZoomToDistance(originalDistance));
        }
    }

    private IEnumerator ZoomToDistance(float targetDistance)
    {
        float velocity = 0f;

        while (Mathf.Abs(cameraOrbital.distance - targetDistance) > 0.01f)
        {
            cameraOrbital.distance = Mathf.SmoothDamp(cameraOrbital.distance, targetDistance, ref velocity, smoothTime);
            yield return null;
        }

        cameraOrbital.distance = targetDistance;
    }
}