using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaBarraOxigênio : MonoBehaviour
{
    public Material oxygenMaterial;
    public float maxOffset = 0.5f;
    public float minOffset = -0.5f;
    public float reductionRate = 0.1f;
    public float delayBeforeStart = 2f;
    public float resetDelay = 3f;
    public GameObject player;
    private float currentOffset;
    private float elapsedTime = 0f;
    private float resetElapsedTime = 0f;
    private bool isResetting = false;
    private bool isDecreasing = true;
    public bool isOxygenDepleted = false;

    private float defaultDecreaseRate;
    private float giantEyeDecreaseRate;

    void Start()
    {
        currentOffset = minOffset;
        UpdateMaterial();
    }

    void Update()
    {
        if (isResetting)
        {
            resetElapsedTime += Time.deltaTime;
            if (resetElapsedTime >= resetDelay)
            {
                isResetting = false;
                isDecreasing = true;
                elapsedTime = 0f;
            }
        }
        else if (isDecreasing && !isOxygenDepleted)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= delayBeforeStart)
            {
                currentOffset += reductionRate * Time.deltaTime;
                currentOffset = Mathf.Clamp(currentOffset, minOffset, maxOffset);
                UpdateMaterial();
                if (currentOffset >= maxOffset)
                {
                    OxygenDepleted();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint") && !isResetting && !isOxygenDepleted)
        {
            currentOffset = minOffset;
            UpdateMaterial();

            isResetting = true;
            isDecreasing = false;
            resetElapsedTime = 0f;
        }
    }

    public void ReduceOnCollision(float amount)
    {
        currentOffset -= amount;
        currentOffset = Mathf.Clamp(currentOffset, minOffset, maxOffset);
        UpdateMaterial();

        if (currentOffset >= maxOffset)
        {
            OxygenDepleted();
        }
    }

    void OxygenDepleted()
    {
        isOxygenDepleted = true;
        player.SetActive(false);
        Invoke("RespawnPlayer", 2f);
    }

    void RespawnPlayer()
    {
        currentOffset = minOffset;
        isOxygenDepleted = false;
        player.SetActive(true);
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        if (oxygenMaterial != null)
        {
            oxygenMaterial.SetFloat("_offset", currentOffset);
        }
        else
        {
            Debug.LogWarning("Oxygen Material not assigned.");
        }
    }

    public void ModifyDecreaseRate(float offset)
    {
        reductionRate = offset;
    }

    public void ModifyDecreaseRateForEyes(float offset)
    {
        reductionRate = offset;
    }

    public void ModifyDecreaseRateForGiantEye(float extraRate)
    {
        giantEyeDecreaseRate = extraRate;
        reductionRate += giantEyeDecreaseRate;
    }

    public void ResetDecreaseRateAfterGiantEye()
    {
        reductionRate = defaultDecreaseRate;
    }
}