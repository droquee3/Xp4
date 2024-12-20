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
    public float collisionSoundVolume = 1.0f; 

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
                PlaySoundWithVolume(collisionSound, collisionSoundVolume);
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

    void PlaySoundWithVolume(AudioClip clip, float volume)
    {
        GameObject audioObject = new GameObject("TempAudio");
        audioObject.transform.position = transform.position;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume; 
        audioSource.spatialBlend = 1.0f;
        audioSource.Play();
    }
}