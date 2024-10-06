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
    private bool eyeActivated = false;
    private float deactivateTimer = 0f;
    private Vector3 pushDirection;
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
            Vector3 relativePosition = other.transform.position - transform.position;

            if (Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.z))
            {
                if (relativePosition.x > 0)
                    pushDirection = Vector3.left;
                else
                    pushDirection = Vector3.right;
            }
            else
            {
                if (relativePosition.z > 0)
                    pushDirection = Vector3.back;
                else
                    pushDirection = Vector3.forward;
            }

            Vector3 eyePosition = transform.position + pushDirection * offsetZ;
            initialEyePosition = new Vector3(eyePosition.x, transform.position.y + offsetY, eyePosition.z);
            eye.transform.position = initialEyePosition;

            eye.SetActive(true);
            eyeActivated = true;
            deactivateTimer = activeTime;

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
                }
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - eye.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        targetRotation *= Quaternion.Euler(90, 0, 0);
        eye.transform.rotation = targetRotation;
    }
}