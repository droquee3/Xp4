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
            randomPosition.y = player.position.y + Random.Range(1f, 2f);

            GameObject eye = Instantiate(eyePrefab, randomPosition, Quaternion.identity);
            spawnedEyes.Add(eye);

            OlhosComportamento eyeBehavior = eye.AddComponent<OlhosComportamento>();
            eyeBehavior.Initialize(player, moveSpeed, reductionAmount, progressBar);

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    void Update()
    {
        if (progressBar.isOxygenDepleted)
        {
            StopAllEyes();
        }
    }
}