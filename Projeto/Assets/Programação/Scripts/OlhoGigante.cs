using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhoGigante : MonoBehaviour
{
    public GameObject eye;
    public Transform player;
    public float activeTime = 5f;
    public float offsetY = 5f;
    public float offsetZ = 10f;
    public ProgressBar oxygenBar;
    public float oxygenIncreaseRate = 0.05f;
    public Vector3 eyeRotationOffset; 
    private bool eyeActivated = false;
    private float deactivateTimer = 0f;
    private Vector3 initialEyePosition;

    void Start()
    {
        if (eye != null)
        {
            eye.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !eyeActivated)
        {
            Vector3 playerDirection = other.transform.forward;

            Vector3 eyePosition = transform.position + playerDirection * offsetZ;
            initialEyePosition = new Vector3(eyePosition.x, transform.position.y + offsetY, eyePosition.z);
            eye.transform.position = initialEyePosition;

            eye.SetActive(true);
            eyeActivated = true;
            deactivateTimer = activeTime;

            if (oxygenBar != null)
            {
                oxygenBar.ModifyDecreaseRateForEyes(oxygenBar.decreaseRate + oxygenIncreaseRate);
            }

            LookAtPlayer();
        }
    }

    void Update()
    {
        if (eyeActivated && player != null)
        {
            LookAtPlayer();

            if (deactivateTimer > 0)
            {
                deactivateTimer -= Time.deltaTime;

                if (deactivateTimer <= 0)
                {
                    eye.SetActive(false);
                    eyeActivated = false;

                    if (oxygenBar != null)
                    {
                        oxygenBar.ModifyDecreaseRateForEyes(oxygenBar.decreaseRate - oxygenIncreaseRate);
                    }
                }
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - eye.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        targetRotation *= Quaternion.Euler(eyeRotationOffset); 
        eye.transform.rotation = targetRotation;
    }
}