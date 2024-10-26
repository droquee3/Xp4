using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhosComportamento : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;
    private float reductionAmount;
    private ProgressBar progressBar;
    private ParticleSystem explosionEffect;

    public void Initialize(Transform player, float moveSpeed, float reductionAmount, ProgressBar progressBar)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.reductionAmount = reductionAmount;
        this.progressBar = progressBar;

        explosionEffect = GetComponentInChildren<ParticleSystem>(true);
        if (explosionEffect != null)
        {
            explosionEffect.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null && !progressBar.isOxygenDepleted)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y += 1.5f;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (explosionEffect != null)
            {
                explosionEffect.transform.parent = null;
                explosionEffect.gameObject.SetActive(true);
                explosionEffect.Play();

              
                Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
            }

            if (progressBar != null) 
            {
                progressBar.ReduceOnCollision(reductionAmount);
            }

            Destroy(gameObject);
        }
    }
}