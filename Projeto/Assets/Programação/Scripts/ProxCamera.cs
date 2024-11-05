using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxCamera : MonoBehaviour
{
    public CameraOrbital cameraOrbital;
    public float zoomOutDistance = 10f;
    public float zoomDuration = 3f;
    public float smoothTime = 0.5f;

    private float originalDistance;
    private Coroutine zoomCoroutine;
    private bool hasZoomedOnce = false;

    void Start()
    {
        if (cameraOrbital != null)
        {
            originalDistance = cameraOrbital.distance;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cameraOrbital != null && !hasZoomedOnce)
        {
            hasZoomedOnce = true;
            if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(HandleZoom());
        }
    }

    private IEnumerator HandleZoom()
    {
        yield return StartCoroutine(ZoomToDistance(zoomOutDistance));

        yield return new WaitForSeconds(zoomDuration);

        yield return StartCoroutine(ZoomToDistance(originalDistance));
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