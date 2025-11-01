using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float JumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
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
        bool wasGrounded = isGrounded;
        // Verificar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log(isGrounded);
        // Movimiento
        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * MovementSpeed, rb.linearVelocity.y);
        
        // Salto solo si est� en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            Debug.Log("Jump!");
            rb.AddForceAtPosition(new Vector2(0, 10f), Vector2.up, ForceMode2D.Impulse);
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            // Verde si está en el suelo, rojo si no
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            // Dibujar línea desde el centro del personaje
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, groundCheck.position);
        }
    }

    internal void ApplyKnockback(Vector2 direction, float finalKnockback, float stunTime)
    {
        throw new NotImplementedException();
    }

    internal bool IsFacingRight()
    {
        throw new NotImplementedException();
    }
}