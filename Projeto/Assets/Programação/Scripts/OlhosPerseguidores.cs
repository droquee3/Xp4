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
    public LayerMask groundLayerMask;
    private bool isSpawning = false;
    private List<GameObject> spawnedEyes = new List<GameObject>();
    private bool playerInsideTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

        while (spawnedEyes.Count < maxNumberOfEyes && playerInsideTrigger)
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
            StartCoroutine(MoveEyeTowardsPlayer(eye));

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }


    IEnumerator MoveEyeTowardsPlayer(GameObject eye)
    {
        while (eye != null && playerInsideTrigger)
        {
            Vector3 direction = (player.position - eye.transform.position).normalized;
            eye.transform.position += direction * moveSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            eye.transform.rotation = Quaternion.Slerp(eye.transform.rotation, targetRotation, 0.1f);

            if (Vector3.Distance(eye.transform.position, player.position) < 0.5f)
            {
                Destroy(eye);
                spawnedEyes.Remove(eye);
            }

            yield return null;
        }

        if (eye != null)
        {
            Destroy(eye);
        }
    }
}