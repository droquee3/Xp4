using UnityEngine;

public class UnderwaterMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidade de movimento debaixo d'água
    public float jumpForce = 5f; // Força do pulo
    public float swimSpeed = 3f; // Velocidade de nado
    public float riseSpeed = 1.5f; // Velocidade de subida ao segurar a barra de espaço
    public float fallMultiplier = 0.5f; // Multiplicador para deixar a queda mais lenta
    public float rotateSpeed = 50f; // Velocidade de rotação
    private bool isGrounded = true; // Verificar se está no chão
    private bool isSwimming = false; // Verifica se o jogador está nadando
    private float lastJumpTime = 0f; // Tempo do último pulo
    private float doubleTapTime = 0.3f; // Tempo limite entre dois toques na barra de espaço para sair do nado
    public Rigidbody rb; // Referência ao Rigidbody do personagem

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody
    }

    void Update()
    {
        // Movimento básico (frente, trás, esquerda, direita)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (!isSwimming)
        {
            // Movimenta o personagem normalmente
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        }
        else
        {
            // Movimentação de nado, pode mover nas 3 direções
            rb.velocity = new Vector3(movement.x * swimSpeed, rb.velocity.y, movement.z * swimSpeed);

            // Se o jogador estiver segurando a barra de espaço, ele sobe devagar
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector3(rb.velocity.x, riseSpeed, rb.velocity.z);
            }
            else
            {
                // Se não estiver segurando a barra de espaço, ele para de subir
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }
        }

        // Rotação do personagem
        float rotation = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // Verifica se o jogador pressiona a barra de espaço para pular ou nadar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                if (Time.time - lastJumpTime < doubleTapTime)
                {
                    ToggleSwimMode(); // Ativa/desativa o modo de nado
                }
                else
                {
                    lastJumpTime = Time.time; // Armazena o tempo do primeiro pulo
                }
            }
        }

        // Se o personagem estiver caindo e não estiver nadando, aplica o multiplicador de queda
        if (rb.velocity.y < 0 && !isSwimming)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void Jump()
    {
        // Aplica a força de pulo
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void ToggleSwimMode()
    {
        isSwimming = !isSwimming; // Alterna entre nadar e não nadar
        if (!isSwimming)
        {
            // Se o jogador parar de nadar, a gravidade volta ao normal
            rb.useGravity = true;
        }
        else
        {
            // Enquanto nadar, a gravidade é desativada e a subida/descida é controlada
            rb.useGravity = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o personagem colidiu com o chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (isSwimming)
            {
                ToggleSwimMode(); // Sai do modo de nado ao tocar o chão
            }
        }
    }
}
