using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhosComportamento : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;
    private float reductionAmount;
    private NovaBarraOxigênio oxygenBar;
    private ParticleSystem explosionEffect;

    public Vector3 rotationOffset;
    public AudioClip collisionSound; 

    public void Initialize(Transform player, float moveSpeed, float reductionAmount, NovaBarraOxigênio oxygenBar)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.reductionAmount = reductionAmount;
        this.oxygenBar = oxygenBar;

        explosionEffect = GetComponentInChildren<ParticleSystem>(true);
        if (explosionEffect != null)
        {
            explosionEffect.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null && !oxygenBar.isOxygenDepleted)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y += 1.5f;
            Vector3 directionToPlayer = (targetPosition - transform.position).normalized;

            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer) * Quaternion.Euler(rotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entrou");

            if (collisionSound != null)
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);
            }

            if (explosionEffect != null)
            {
                explosionEffect.transform.parent = null;
                explosionEffect.gameObject.SetActive(true);
                explosionEffect.Play();

                Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
            }

            if (oxygenBar != null)
            {
                oxygenBar.ReduceOnCollision(-reductionAmount);
            }

            Destroy(gameObject);
        }
    }
}