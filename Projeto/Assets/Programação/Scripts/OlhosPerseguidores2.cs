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
    public float minDistanceBetweenEyes = 10f; 
    public NovaBarraOxigênio oxygenBar;
    public float reductionAmount = 1.7f;

    private bool isSpawning = false;
    private List<GameObject> spawnedEyes = new List<GameObject>();
    private bool playerInsideTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !oxygenBar.isOxygenDepleted)
        {
            playerInsideTrigger = true;
            if (!isSpawning && spawnedEyes.Count < maxNumberOfEyes)
            {
                StartCoroutine(DelayedSpawnEyes());
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

    IEnumerator DelayedSpawnEyes()
    {
        yield return new WaitForSeconds(3f); 

        if (playerInsideTrigger && !isSpawning && spawnedEyes.Count < maxNumberOfEyes)
        {
            StartCoroutine(SpawnEyes());
        }
    }

    IEnumerator SpawnEyes()
    {
        isSpawning = true;

        while (spawnedEyes.Count < maxNumberOfEyes && playerInsideTrigger && !oxygenBar.isOxygenDepleted)
        {
            Vector3 randomPosition;
            bool positionIsValid;

            do
            {
                positionIsValid = true;
                randomPosition = player.position + (Random.insideUnitSphere * spawnRadius);
                randomPosition.y = player.position.y + Random.Range(1f, 2f);

                foreach (var existingEye in spawnedEyes) 
                {
                    if (existingEye != null && Vector3.Distance(randomPosition, existingEye.transform.position) < minDistanceBetweenEyes)
                    {
                        positionIsValid = false;
                        break;
                    }
                }
            } while (!positionIsValid);

            GameObject newEye = Instantiate(eyePrefab, randomPosition, Quaternion.identity); 
            spawnedEyes.Add(newEye);

            OlhosComportamento eyeBehavior = newEye.AddComponent<OlhosComportamento>();
            eyeBehavior.Initialize(player, moveSpeed, reductionAmount, oxygenBar);

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
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

    void Update()
    {
        if (oxygenBar.isOxygenDepleted)
        {
            StopAllEyes();
        }
    }
}