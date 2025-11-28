using System;
using System.Collections; // Necesario para IEnumerator
using UnityEngine.InputSystem;
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

    // --- NUEVA VARIABLE ---
    // Indica si el jugador ha sido golpeado y no puede moverse
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        // Configuración inicial del respawn
        if (respawnPosition == Vector3.zero)
            respawnPosition = new Vector3(0f, 11.04f, 0f);

        // --- CREACIÓN AUTOMÁTICA DE CHECKS (Tu código original) ---
        if (groundCheck == null || groundCheck.localPosition.y >= 0)
        {
            Transform existingCheck = transform.Find("GroundCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.parent = transform;
            groundCheckObj.transform.localPosition = new Vector3(0, -1f, 0);
            groundCheck = groundCheckObj.transform;
        }
        if (leftCheck == null)
        {
            Transform existingCheck = transform.Find("LeftCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            GameObject leftCheckObj = new GameObject("LeftCheck");
            leftCheckObj.transform.parent = transform;
            leftCheckObj.transform.localPosition = new Vector3(-1f, 0, 0);
            leftCheck = leftCheckObj.transform;
        }
        if (rightCheck == null)
        {
            Transform existingCheck = transform.Find("RightCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            GameObject rightCheckObj = new GameObject("RightCheck");
            rightCheckObj.transform.parent = transform;
            rightCheckObj.transform.localPosition = new Vector3(1f, 0, 0);
            rightCheck = rightCheckObj.transform;
        }
        if (ceilingCheck == null)
        {
            Transform existingCheck = transform.Find("CeilingCheck");
            if (existingCheck != null) DestroyImmediate(existingCheck.gameObject);
            GameObject ceilingCheckObj = new GameObject("CeilingCheck");
            ceilingCheckObj.transform.parent = transform;
            ceilingCheckObj.transform.localPosition = new Vector3(0, 1f, 0);
            ceilingCheck = ceilingCheckObj.transform;
        }
        // ---------------------------------------------------------

        if (groundLayer == 0) Debug.LogWarning("GroundLayer no está configurado!");
        if (((1 << gameObject.layer) & groundLayer) != 0) Debug.LogError("¡ERROR! El Player está incluido en GroundLayer.");
    }

    void FixedUpdate()
    {
        if (isDead) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWallLeft = Physics2D.Raycast(leftCheck.position, Vector2.left, wallCheckDistance, groundLayer);
        isTouchingWallRight = Physics2D.Raycast(rightCheck.position, Vector2.right, wallCheckDistance, groundLayer);
        isTouchingCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, groundLayer);

        isGrounded = isGrounded || isTouchingWallLeft || isTouchingWallRight || isTouchingCeiling;
    }

    void Update()
    {
        if (isDead) return;

        // --- CAMBIO IMPORTANTE ---
        // Si estamos aturdidos por un golpe, SALIMOS de Update.
        // Esto evita que el input del teclado sobrescriba la fuerza del golpe.
        if (isKnockedBack) return;
        // -------------------------

        // Reset jump count
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpCount = 0;
        }

        // Movimiento
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput.x * MovementSpeed, rb.linearVelocity.y);

        if (moveInput.x > 0 && !facingRight) Flip();
        else if (moveInput.x < 0 && facingRight) Flip();

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

    // --- IMPLEMENTACIÓN DEL KNOCKBACK ---
    internal void ApplyKnockback(Vector2 direction, float finalKnockback, float stunTime)
    {
        // 1. Activar estado de aturdimiento
        isKnockedBack = true;

        // 2. Detener al jugador para que el golpe sea seco
        rb.linearVelocity = Vector2.zero;

        // 3. Aplicar la fuerza
        rb.AddForce(direction * finalKnockback, ForceMode2D.Impulse);

        Debug.Log("¡Player empujado! Fuerza: " + finalKnockback);

        // 4. Iniciar la cuenta atrás para recuperar el control
        StartCoroutine(KnockbackRoutine(stunTime));
    }

    // Corrutina para esperar el tiempo de aturdimiento
    private IEnumerator KnockbackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isKnockedBack = false; // Recuperar control
        Debug.Log("Player recuperó el control");
    }
    // -------------------------------------

    internal bool IsFacingRight()
    {
        return facingRight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlastZone"))
        {
            Debug.Log($"¡Player entró en Blast Zone: {other.gameObject.name}!");
            Die();
        }
        // HE QUITADO 'Update()' DE AQUÍ. JAMÁS LLAMES A UPDATE MANUALMENTE.
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("¡Player ha muerto! Respawn en posición: " + respawnPosition);

        rb.linearVelocity = Vector2.zero;
        Invoke("Respawn", respawnDelay);
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
        rb.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        jumpCount = 0;
        currentGravityDirection = Vector2.down;
        facingRight = true;
        isDead = false;

        // Asegurarnos de que no respawneamos aturdidos
        isKnockedBack = false;

        Debug.Log("Player respawneado");
    }

    public bool IsGrounded() => isGrounded;
    public bool IsTouchingWallLeft() => isTouchingWallLeft;
    public bool IsTouchingWallRight() => isTouchingWallRight;
    public bool IsTouchingCeiling() => isTouchingCeiling;

    // GIZMOS (Sin cambios)
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (leftCheck != null)
        {
            Gizmos.color = isTouchingWallLeft ? Color.green : Color.red;
            Gizmos.DrawRay(leftCheck.position, Vector2.left * wallCheckDistance);
        }
        if (rightCheck != null)
        {
            Gizmos.color = isTouchingWallRight ? Color.green : Color.red;
            Gizmos.DrawRay(rightCheck.position, Vector2.right * wallCheckDistance);
        }
        if (ceilingCheck != null)
        {
            Gizmos.color = isTouchingCeiling ? Color.green : Color.red;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }
    }
}