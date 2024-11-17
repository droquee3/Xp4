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
    public float resetDelay = 1f;
    public GameObject player;
    public AudioClip oxygenDepletedSound;
    public float deathSoundVolume = 1.0f;
    private Vector3 checkpointPosition;
    private float currentOffset;
    private float elapsedTime = 0f;
    private float resetElapsedTime = 0f;
    private bool isResetting = false;
    private bool isDecreasing = true;
    public bool isOxygenDepleted = false;
    private Animator animator;
    private float defaultDecreaseRate;
    private float giantEyeDecreaseRate;
   
    void Start()
    {
        currentOffset = minOffset;
        UpdateMaterial();
        animator = player.GetComponent<Animator>();
        checkpointPosition = player.transform.position;
    }

    public void SetCheckpointPosition(Vector3 position)
    {
        checkpointPosition = position; 
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
        animator.SetTrigger("Death");

        if (oxygenDepletedSound != null)
        {
            PlaySoundWithVolume(oxygenDepletedSound, deathSoundVolume);
        }

        StartCoroutine(WaitForDeathAnimation());
    }

    void PlaySoundWithVolume(AudioClip clip, float volume)
    {
        
        GameObject audioObject = new GameObject("TempAudio");
        audioObject.transform.position = transform.position;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume; 
        audioSource.Play();
        Destroy(audioObject, clip.length); 
    }

    IEnumerator WaitForDeathAnimation()
    {
        animator.SetFloat("Speed", 1.0f);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(2f);

        player.SetActive(false);

        Invoke("RespawnPlayer", 2f);
    }

    void RespawnPlayer()
    {
        currentOffset = minOffset;
        isOxygenDepleted = false;
        player.transform.position = checkpointPosition; 
        player.SetActive(true);

        animator.SetTrigger("IsGrounded");
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

    public void SetOxygenToMinOffset(float minOffset)
    {
        currentOffset = minOffset;
        UpdateMaterial();
    }

    public void RestartOxygenDelay()
    {
        resetElapsedTime = 0f;
        isResetting = true; 
        isDecreasing = false;
        Debug.Log("Atraso do oxigênio reiniciado após o checkpoint!");
    }
}