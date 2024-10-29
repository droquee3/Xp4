using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class ZoomCamera : MonoBehaviour
{
    public CameraOrbital cameraOrbital;
    public Shaker cameraShaker; // Referência ao Shaker do MilkShake
    public ShakePreset shakePreset; // Preset de tremor configurável no inspector

    public float zoomOutDistance = 10f;
    public float zoomDuration = 3f;
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
            zoomCoroutine = StartCoroutine(HandleZoom());
        }
    }

    private IEnumerator HandleZoom()
    {
        // Inicia o tremor da câmera ao entrar no collider
        cameraShaker.Shake(shakePreset);

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