using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhoComportamento2 : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;
    private float reductionOffset;
    private float normalDecreaseRate;
    private ProgressBar progressBar;
    private ParticleSystem explosionEffect;

    public float proximityThreshold = 5f;
    public Vector3 rotationOffset;

    public void Initialize(Transform player, float moveSpeed, float reductionOffset, float normalDecreaseRate, ProgressBar progressBar, float proximityThreshold)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.reductionOffset = reductionOffset;
        this.normalDecreaseRate = normalDecreaseRate;
        this.progressBar = progressBar;
        this.proximityThreshold = proximityThreshold;

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
            Vector3 directionToPlayer = (targetPosition - transform.position).normalized;

            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer) * Quaternion.Euler(rotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < proximityThreshold)
            {
                progressBar.ModifyDecreaseRateForEyes(reductionOffset);
            }
            else
            {
                progressBar.ModifyDecreaseRate(normalDecreaseRate);
            }
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
                progressBar.ReduceOnCollision(reductionOffset);
            }

            Destroy(gameObject);
        }
    }
}