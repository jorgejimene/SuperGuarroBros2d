using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float JumpForce = 10f;

    [Header("Ground Check")]
    // public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Verificar si est� en el suelo
        // isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento
        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * MovementSpeed, rb.linearVelocity.y);

        // Salto solo si est� en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }
    }


    public void ApplyKnockback(Vector2 direction, float forceStrength)
    {
        Rigidbody2D myRb = GetComponent<Rigidbody2D>();

        if (myRb != null)
        {
            // 1. Stop the player momentarily so the punch feels heavy
            myRb.linearVelocity = Vector2.zero;

            // 2. Add the force instantly (Impulse)
            // direction * forceStrength gives the total push vector
            myRb.AddForce(direction * forceStrength, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        // if (groundCheck != null)
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        // }
    }
}