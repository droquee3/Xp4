using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float maxHeight = 1f;
    public float decreaseRate;
    public float delayBeforeStart = 2f;
    public float resetDelay = 3f;
    public GameObject player;
    private Vector3 respawnPosition;

    private float currentHeight;
    private float elapsedTime = 0f;
    private float resetElapsedTime = 0f;
    private bool isResetting = false;
    private bool isDecreasing = true;
    public bool isOxygenDepleted = false;

    private float defaultDecreaseRate;
    private float giantEyeDecreaseRate;

    void Start()
    {
        currentHeight = maxHeight;
        respawnPosition = player.transform.position;
        UpdateScale();
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
                currentHeight -= decreaseRate * Time.deltaTime;
                currentHeight = Mathf.Clamp(currentHeight, 0, maxHeight);
                UpdateScale();
                if (currentHeight <= 0)
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
            currentHeight = maxHeight;
            UpdateScale();
            respawnPosition = other.transform.position;

            isResetting = true;
            isDecreasing = false;
            resetElapsedTime = 0f;
        }
    }

    public void ReduceOnCollision(float amount)
    {
        currentHeight -= amount;
        currentHeight = Mathf.Clamp(currentHeight, 0, maxHeight);
        UpdateScale();

        if (currentHeight <= 0)
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
        player.transform.position = respawnPosition;
        currentHeight = maxHeight;
        isOxygenDepleted = false;
        player.SetActive(true);
        enabled = true;
    }

    void UpdateScale()
    {
        transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
    }

    public void ModifyDecreaseRate(float offset)
    {
        decreaseRate = 0f + offset;
    }


    public void ModifyDecreaseRateForEyes(float offset)
    {
        decreaseRate = offset; 
    }

    public void ModifyDecreaseRateForGiantEye(float extraRate)
    {
        giantEyeDecreaseRate = extraRate;
        decreaseRate += giantEyeDecreaseRate; 
    }

    public void ResetDecreaseRateAfterGiantEye()
    {
        decreaseRate = defaultDecreaseRate; 
    }
}
