using UnityEngine;

public class UnderwaterMovement : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float jumpForce = 5f; 
    public float swimSpeed = 3f; 
    public float riseSpeed = 1.5f; 
    public float fallMultiplier = 0.5f; 
    public float rotateSpeed = 50f; 
    private bool isGrounded = true; 
    private bool isSwimming = false; 
    private float lastJumpTime = 0f; 
    private float doubleTapTime = 0.3f; 
    public Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.useGravity = true; 
    }

    void Update()
    {
       
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (!isSwimming)
        {
         
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        }
        else
        {
            
            rb.velocity = new Vector3(movement.x * swimSpeed, rb.velocity.y, movement.z * swimSpeed);

          
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector3(rb.velocity.x, riseSpeed, rb.velocity.z);
            }
            else
            {
               
                rb.velocity = new Vector3(rb.velocity.x, -0.1f, rb.velocity.z);
            }
        }

       
        float rotation = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        
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
                    ToggleSwimMode(); 
                }
                else
                {
                    lastJumpTime = Time.time; 
                }
            }
        }

      
        if (rb.velocity.y < 0 && !isSwimming)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void Jump()
    {
       
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void ToggleSwimMode()
    {
        isSwimming = !isSwimming;

        if (!isSwimming)
        {
            
            rb.useGravity = true;
        }
     
    }

    private void OnCollisionEnter(Collision collision)
    {
     
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (isSwimming)
            {
                ToggleSwimMode(); 
            }
        }
    }
}