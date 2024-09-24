using UnityEngine;
using UnityEngine.InputSystem;

public class UnderwaterMovement : MonoBehaviour {
    public float moveSpeed = 2f; 
    public float jumpForce = 5f; 
    public float swimSpeed = 3f; 
    public float riseSpeed = 1.5f; 
    public float fallMultiplier = 0.5f; 
    public Rigidbody rb; 
    public Transform cameraTransform;  // Referência para a câmera
    private bool isGrounded = true; 
    private bool isSwimming = false; 
    private float lastJumpTime = 0f; 
    private float doubleTapTime = 0.3f; 

    void Start() {
        rb = GetComponent<Rigidbody>(); 
        rb.useGravity = true; 

        if (cameraTransform == null) {
            cameraTransform = GetComponent<Transform>();   // Pega a câmera principal se não estiver configurada
        }
    }

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Direção baseada na câmera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Normaliza os vetores para que a altura (eixo y) não influencie a direção
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Movimentação baseada na câmera
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        if (!isSwimming) {
            // Movimentação no solo baseada na direção da câmera
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        } else {
            // Movimentação na água baseada na câmera
            rb.velocity = new Vector3(movement.x * swimSpeed, rb.velocity.y, movement.z * swimSpeed);

            // Subir ao pressionar o botão de pulo
            if (Input.GetButton("Jump")) {
                rb.velocity = new Vector3(rb.velocity.x, riseSpeed, rb.velocity.z);
            } else {
                // Caso não esteja pressionando o botão de pulo, aplica uma leve queda
                rb.velocity = new Vector3(rb.velocity.x, -0.1f, rb.velocity.z);
            }
        }

        // Pular com o botão "Jump"
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                Jump();
            } else {
                if (Time.time - lastJumpTime < doubleTapTime) {
                    ToggleSwimMode(); 
                } else {
                    lastJumpTime = Time.time; 
                }
            }
        }

        // Aumentar o efeito da gravidade quando caindo (fora da água)
        if (rb.velocity.y < 0 && !isSwimming) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void ToggleSwimMode() {
        isSwimming = !isSwimming;

        if (!isSwimming) {
            rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            if (isSwimming) {
                ToggleSwimMode(); 
            }
        }
    }
}
