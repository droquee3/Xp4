using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhosPerseguidores : MonoBehaviour
{
    public GameObject eyePrefab;
    public Transform player;
    public float spawnRadius = 5f;
    public float moveSpeed = 2f;
    public int maxNumberOfEyes = 5;
    public float spawnInterval = 1f;
    public ProgressBar progressBar;
    public float proximityThreshold = 5f;
    public float reductionOffset = 0.2f;
    public float normalDecreaseRate = 0.01f;

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
            progressBar.ModifyDecreaseRate(normalDecreaseRate);
        }
    }

    void StopAllEyes()
    {
        for (int i = spawnedEyes.Count - 1; i >= 0; i--)
        {
            if (spawnedEyes[i] != null)
            {
                Destroy(spawnedEyes[i]);
            }
            spawnedEyes.RemoveAt(i);
        }
        isSpawning = false;
    }

    IEnumerator SpawnEyes()
    {
        isSpawning = true;

        while (spawnedEyes.Count < maxNumberOfEyes && playerInsideTrigger && !progressBar.isOxygenDepleted)
        {
            Vector3 randomPosition;
            bool validPosition = false;

            do
            {
                randomPosition = player.position + (Random.insideUnitSphere * spawnRadius);
                randomPosition.y = Random.Range(player.position.y + 1f, player.position.y + 2f);

                validPosition = true;

                foreach (var spawnedEye in spawnedEyes)
                {
                    if (Vector3.Distance(spawnedEye.transform.position, randomPosition) < 1f)
                    {
                        validPosition = false;
                        break;
                    }
                }

            } while (!validPosition);

            GameObject eye = Instantiate(eyePrefab, randomPosition, Quaternion.identity);
            spawnedEyes.Add(eye);

            var eyeBehavior = eye.AddComponent<OlhoComportamento2>();
            eyeBehavior.Initialize(player, moveSpeed, reductionOffset, normalDecreaseRate, progressBar, proximityThreshold);

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