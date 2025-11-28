using System;
using UnityEngine.InputSystem; // ¡Añade esta línea!
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float JumpForce = 10f;
    

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    [Header("Double Jump")]
    public bool allowDoubleJump = true;
    public float doubleJumpForce = 8f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 1f;
    public LayerMask groundLayer;
    
    [Header("Wall & Ceiling Check")]
    public Transform leftCheck;
    public Transform rightCheck;
    public Transform ceilingCheck;
    public float wallCheckDistance = 0.2f;
    public float ceilingCheckRadius = 0.5f;

    [Header("Respawn System")]
    public Vector3 respawnPosition = Vector3.zero;
    public float respawnDelay = 2f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;
    private bool isTouchingCeiling;
    private bool facingRight = true;
    private int jumpCount = 0;

    private Vector2 currentGravityDirection = Vector2.down;
    private bool isDead = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerInput = GetComponent<PlayerInput>();
        
        // Obtener las acciones específicas de este player
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

    	respawnPosition = new Vector3(0f, 11.04f, 0f);	

        // Crear GroundCheck automáticamente si no existe O si está mal posicionado
        if (groundCheck == null || groundCheck.localPosition.y >= 0)
        {
            Transform existingCheck = transform.Find("GroundCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.parent = transform;
            groundCheckObj.transform.localPosition = new Vector3(0, -1f, 0);
            groundCheck = groundCheckObj.transform;
            Debug.Log("GroundCheck creado automáticamente");
        }
        
        // Crear LeftCheck automáticamente
        if (leftCheck == null)
        {
            Transform existingCheck = transform.Find("LeftCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            
            GameObject leftCheckObj = new GameObject("LeftCheck");
            leftCheckObj.transform.parent = transform;
            leftCheckObj.transform.localPosition = new Vector3(-1f, 0, 0);
            leftCheck = leftCheckObj.transform;
            Debug.Log("LeftCheck creado automáticamente");
        }
        
        // Crear RightCheck automáticamente
        if (rightCheck == null)
        {
            Transform existingCheck = transform.Find("RightCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            
            GameObject rightCheckObj = new GameObject("RightCheck");
            rightCheckObj.transform.parent = transform;
            rightCheckObj.transform.localPosition = new Vector3(1f, 0, 0);
            rightCheck = rightCheckObj.transform;
            Debug.Log("RightCheck creado automáticamente");
        }
        
        // Crear CeilingCheck automáticamente
        if (ceilingCheck == null)
        {
            Transform existingCheck = transform.Find("CeilingCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            
            GameObject ceilingCheckObj = new GameObject("CeilingCheck");
            ceilingCheckObj.transform.parent = transform;
            ceilingCheckObj.transform.localPosition = new Vector3(0, 1f, 0);
            ceilingCheck = ceilingCheckObj.transform;
            Debug.Log("CeilingCheck creado automáticamente");
        }
        
        if (groundLayer == 0)
            Debug.LogWarning("GroundLayer no está configurado!");
        
        if (((1 << gameObject.layer) & groundLayer) != 0)
        {
            Debug.LogError("¡ERROR! El Player está incluido en GroundLayer.");
        }
    }
    
    void FixedUpdate()
    {
	if (isDead) return;

        // Verificar colisiones en todos los lados
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWallLeft = Physics2D.Raycast(leftCheck.position, Vector2.left, wallCheckDistance, groundLayer);
        isTouchingWallRight = Physics2D.Raycast(rightCheck.position, Vector2.right, wallCheckDistance, groundLayer);
        isTouchingCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, groundLayer);
        
        // Si el cubo está rotado, cualquier lado puede ser el "suelo"
        // Por eso consideramos que está "grounded" si toca cualquier superficie
        isGrounded = isGrounded || isTouchingWallLeft || isTouchingWallRight || isTouchingCeiling;
    }
    
    void Update()
    {
	if (isDead) return;

        // Reset jump count when grounded
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            if (jumpCount != 0)
            {
                Debug.Log($"Aterrizó! Reset jumpCount de {jumpCount} a 0");
            }
            jumpCount = 0;
        }
        
        // Movimiento
        Vector2 moveInput = moveAction.ReadValue<Vector2>();        
        rb.linearVelocity = new Vector2(moveInput.x * MovementSpeed, rb.linearVelocity.y);
                
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
        
        // Salto
        if (jumpAction.triggered)
        {
            if (isGrounded && jumpCount == 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
                jumpCount = 1;
            }
            else if (allowDoubleJump && !isGrounded && jumpCount == 1)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                jumpCount = 2;
            }
        }
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    void OnDrawGizmosSelected()
    {
        // Ground Check
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        // Left Wall Check
        if (leftCheck != null)
        {
            Gizmos.color = isTouchingWallLeft ? Color.green : Color.red;
            Gizmos.DrawRay(leftCheck.position, Vector2.left * wallCheckDistance);
            Gizmos.DrawWireSphere(leftCheck.position + (Vector3.left * wallCheckDistance), 0.1f);
        }
        
        // Right Wall Check
        if (rightCheck != null)
        {
            Gizmos.color = isTouchingWallRight ? Color.green : Color.red;
            Gizmos.DrawRay(rightCheck.position, Vector2.right * wallCheckDistance);
            Gizmos.DrawWireSphere(rightCheck.position + (Vector3.right * wallCheckDistance), 0.1f);
        }
        
        // Ceiling Check
        if (ceilingCheck != null)
        {
            Gizmos.color = isTouchingCeiling ? Color.green : Color.red;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }
    }
    
    internal void ApplyKnockback(Vector2 direction, float finalKnockback, float stunTime)
    {
        throw new NotImplementedException();
    }
    
    internal bool IsFacingRight()
    {
        return facingRight;
    }
void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto con el que colisionamos es una Blast Zone
        if (other.CompareTag("BlastZone"))
        {
            Debug.Log($"¡Player entró en Blast Zone: {other.gameObject.name}!");
            Die();
        }

	Update();
    }

private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("¡Player ha muerto! Respawn en posición: " + respawnPosition);
        
        // Detener el movimiento
        rb.linearVelocity = Vector2.zero;
        
        
        // Invocar el respawn después de un delay
        Invoke("Respawn", respawnDelay);
    }
    
    private void Respawn()
    {
        // Resetear posición
        transform.position = respawnPosition;
        
        // Resetear velocidad y rotación
        rb.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        
        // Resetear variables de estado
        jumpCount = 0;
        currentGravityDirection = Vector2.down;
        facingRight = true;
        
        // Reactivar el control
        isDead = false;
        
        Debug.Log("Player respawneado en posición inicial");
    }
    
    // Métodos públicos para acceder a las colisiones
    public bool IsGrounded() => isGrounded;
    public bool IsTouchingWallLeft() => isTouchingWallLeft;
    public bool IsTouchingWallRight() => isTouchingWallRight;
    public bool IsTouchingCeiling() => isTouchingCeiling;
}