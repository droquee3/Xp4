using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UnderwaterMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float swimSpeed = 3f;
    public float riseSpeed = 1.5f;
    public float fallSpeed = 1.5f;
    public float swimFallSpeed = 1.0f;
    public float rotationSpeed = 5f;
    public float minSwimHeight = 0.5f;
    public Rigidbody rb;
    public Transform cameraTransform;
    private bool isGrounded = true;
    private bool isSwimming = false;
    private float startYPosition;
    public NovaBarraOxigênio progressBar;
    public float swimOxygenReductionRate = 0.02f;
    private float previousYPosition;
    private float defaultOxygenReductionRate = 0.001f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }

        previousYPosition = transform.position.y;
        progressBar.ModifyDecreaseRate(defaultOxygenReductionRate);
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        if (!isSwimming)
        {
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);

            if (rb.velocity.y < 0 && !isGrounded)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
                if (rb.velocity.y < -0.1f)
                {
                    //Debug.Log("[Nado Automático] Entrando no modo de nado devido à queda.");
                    ToggleSwimMode();
                }
            }
        }
        else
        {
            rb.velocity = new Vector3(movement.x * swimSpeed, rb.velocity.y, movement.z * swimSpeed);

            if (Input.GetButton("Jump"))
            { 
                rb.velocity = new Vector3(rb.velocity.x, riseSpeed, rb.velocity.z);
                animator.SetBool("IsFalling", false);
            }
            else
            { 
                rb.velocity = new Vector3(rb.velocity.x, -swimFallSpeed, rb.velocity.z);
                animator.SetBool("IsFalling", true);
            }

            UpdateOxygenAndYPosition();
        }

        HandleRotationAndAnimation(movement);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        animator.SetBool("IsGrounded", false);
        startYPosition = transform.position.y;
        //Debug.Log($"[Jump] StartY: {startYPosition}");
    }

    void ToggleSwimMode()
    {
        isSwimming = !isSwimming;
        //Debug.Log($"[ToggleSwimMode] Swim Mode Ativo: {isSwimming}");

        rb.useGravity = !isSwimming;
        progressBar.ModifyDecreaseRate(defaultOxygenReductionRate);
    }

    void UpdateOxygenAndYPosition()
    {
        float currentYPosition = transform.position.y;
        if (currentYPosition > previousYPosition)
            progressBar.ModifyDecreaseRate(swimOxygenReductionRate);
        else
            progressBar.ModifyDecreaseRate(defaultOxygenReductionRate);

        previousYPosition = currentYPosition;
    }

    void HandleRotationAndAnimation(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Button1") || collision.gameObject.CompareTag("Button2"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
            if (isSwimming) ToggleSwimMode();
            //Debug.Log("[Collision] Colidiu com Ground. Modo de nado desativado.");
        }
    }
}
