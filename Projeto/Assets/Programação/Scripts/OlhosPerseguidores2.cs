using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhosPerseguidores2 : MonoBehaviour
{
    public GameObject eyePrefab;
    public Transform player;
    public float spawnRadius = 5f;
    public float moveSpeed = 2f;
    public int maxNumberOfEyes = 5;
    public float spawnInterval = 1f;
    public ProgressBar progressBar;
    public float reductionAmount = 1.7f;

    private bool isSpawning = false;
    private List<GameObject> spawnedEyes = new List<GameObject>();
    private bool playerInsideTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !progressBar.isOxygenDepleted)
        {
            playerInsideTrigger = true;
            if (!isSpawning && spawnedEyes.Count < maxNumberOfEyes)
            {
                StartCoroutine(SpawnEyes());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            StopAllEyes();
        }
    }

    void StopAllEyes()
    {
        foreach (var eye in spawnedEyes)
        {
            if (eye != null)
            {
                Destroy(eye);
            }
        }
        spawnedEyes.Clear();
        isSpawning = false;
    }

    IEnumerator SpawnEyes()
    {
        isSpawning = true;

        while (spawnedEyes.Count < maxNumberOfEyes && playerInsideTrigger && !progressBar.isOxygenDepleted)
        {
            Vector3 randomPosition = player.position + (Random.insideUnitSphere * spawnRadius);
            randomPosition.y = player.position.y + Random.Range(1f, 2f); // Manter a altura do jogador + uma pequena variação

            GameObject eye = Instantiate(eyePrefab, randomPosition, Quaternion.identity);
            spawnedEyes.Add(eye);

            ParticleSystem explosionEffect = eye.GetComponentInChildren<ParticleSystem>(true);
            if (explosionEffect != null) explosionEffect.gameObject.SetActive(false);

            StartCoroutine(MoveEyeTowardsPlayer(eye, explosionEffect));

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    IEnumerator MoveEyeTowardsPlayer(GameObject eye, ParticleSystem explosionEffect)
    {
        while (eye != null && playerInsideTrigger && !progressBar.isOxygenDepleted)
        {
            Vector3 direction = (player.position - eye.transform.position).normalized;
            eye.transform.position += direction * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            eye.transform.rotation = Quaternion.Slerp(eye.transform.rotation, targetRotation, 0.1f);

            if (Vector3.Distance(eye.transform.position, player.position) < 0.5f)
            {
                if (explosionEffect != null)
                {
                    explosionEffect.gameObject.SetActive(true);
                    explosionEffect.transform.position = eye.transform.position; 
                    explosionEffect.Play();
                }

                spawnedEyes.Remove(eye);
                Destroy(eye); 
                progressBar.ReduceOnCollision(reductionAmount);
                yield break;
            }

            yield return null;
        }

        if (eye != null)
        {
            Destroy(eye);
        }
    }

    void Update()
    {
        if (progressBar.isOxygenDepleted)
        {
            StopAllEyes();
        }
    }
}