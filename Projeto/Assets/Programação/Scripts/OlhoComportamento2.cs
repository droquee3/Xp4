using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhoComportamento2 : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;
    private float reductionOffset;
    private float normalDecreaseRate;
    private NovaBarraOxigênio oxygenBar; 
    private ParticleSystem explosionEffect;

    public float proximityThreshold = 5f;
    public Vector3 rotationOffset;

    
    public void Initialize(Transform player, float moveSpeed, float reductionOffset, float normalDecreaseRate, NovaBarraOxigênio oxygenBar, float proximityThreshold)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.reductionOffset = Mathf.Abs(reductionOffset); 
        this.normalDecreaseRate = -Mathf.Abs(normalDecreaseRate);
        this.oxygenBar = oxygenBar; 
        this.proximityThreshold = proximityThreshold;

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

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < proximityThreshold)
            {
                oxygenBar.ModifyDecreaseRateForEyes(reductionOffset);
            }
            else
            {
                oxygenBar.ModifyDecreaseRate(normalDecreaseRate);
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

            Destroy(gameObject);
        }
    }
}