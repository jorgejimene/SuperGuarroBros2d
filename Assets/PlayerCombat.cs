using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Ataques")]
    public float lightAttackForce = 8f;
    public float heavyAttackForce = 15f;
    public float lightAttackDamage = 5f;
    public float heavyAttackDamage = 12f;
    public float attackRange = 1.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Cooldowns")]
    public float attackCooldown = 0.3f;
    private float nextAttackTime = 0f;

    [Header("Sistema de Da o")]
    public float damagePercent = 0f;
    public float knockbackMultiplier = 0.02f; // Cuanto m s da o, m s knockback

    [Header("Efectos Visuales")]
    public SpriteRenderer sprite;
    public ParticleSystem hitEffect;

    private PlayerMovement movement;
    private Animator animator;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();

        // Crear AttackPoint autom ticamente si no existe
        if (attackPoint == null)
        {
            GameObject ap = new GameObject("AttackPoint");
            ap.transform.parent = transform;
            ap.transform.localPosition = new Vector3(0.8f, 0, 0); // Ajusta seg n tu personaje
            attackPoint = ap.transform;
            Debug.Log("AttackPoint creado autom ticamente");
        }
    }

    void Update()
    {
        // Ataque ligero
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= nextAttackTime)
        {
            LightAttack();
            nextAttackTime = Time.time + attackCooldown;
        }

        // Ataque fuerte
        if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextAttackTime)
        {
            HeavyAttack();
            nextAttackTime = Time.time + attackCooldown;
        }

        // Ataque especial (hacia arriba)
        if (Input.GetKeyDown(KeyCode.C) && Time.time >= nextAttackTime)
        {
            UpAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void LightAttack()
    {
        if (animator != null) animator.SetTrigger("LightAttack");
        PerformAttack(lightAttackForce, lightAttackDamage, Vector2.right, 0.15f);
    }

    void HeavyAttack()
    {
        if (animator != null) animator.SetTrigger("HeavyAttack");
        PerformAttack(heavyAttackForce, heavyAttackDamage, new Vector2(1f, 0.5f), 0.3f);
    }

    void UpAttack()
    {
        if (animator != null) animator.SetTrigger("UpAttack");
        PerformAttack(heavyAttackForce * 0.8f, heavyAttackDamage * 0.8f, Vector2.up, 0.25f);
    }

    void PerformAttack(float force, float damage, Vector2 knockbackDir, float stunTime)
    {
        Vector2 attackPos = attackPoint != null ? attackPoint.position : transform.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject)
            {
                PlayerCombat enemyCombat = enemy.GetComponent<PlayerCombat>();
                if (enemyCombat != null)
                {
                    // Calcular direcci n del knockback
                    Vector2 direction = (enemy.transform.position - transform.position).normalized;

                    // Si el ataque tiene direcci n espec fica, usarla
                    if (knockbackDir != Vector2.right)
                    {
                        direction = knockbackDir.normalized;
                    }

                    // Ajustar direcci n horizontal seg n hacia d nde mira
                    if (!movement.IsFacingRight())
                    {
                        direction.x *= -1;
                    }

                    enemyCombat.TakeDamage(damage, direction, force, stunTime);
                }
            }
        }

        // Efecto visual
        if (hitEffect != null && hitEnemies.Length > 0)
        {
            hitEffect.Play();
        }
    }

    public void TakeDamage(float damage, Vector2 direction, float baseForce, float stunTime)
    {
        // Aumentar el porcentaje de da o
        damagePercent += damage;

        // Calcular knockback final basado en el da o acumulado
        float finalKnockback = baseForce * (1 + damagePercent * knockbackMultiplier);

        // Aplicar knockback
        movement.ApplyKnockback(direction, finalKnockback, stunTime);

        // Efecto visual de da o
        StartCoroutine(DamageFlash());

        // Log para debug
        Debug.Log($"{gameObject.name} Da o: {damagePercent:F1}% - Knockback: {finalKnockback:F1}");

        // Verificar si cay  del mapa
        CheckDeath();
    }

    System.Collections.IEnumerator DamageFlash()
    {
        if (sprite != null)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
        }
    }

    void CheckDeath()
    {
        if (transform.position.y < -20f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} ha sido eliminado!");
        damagePercent = 0f;
        transform.position = new Vector3(0, 5, 0); // Respawn
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 attackPos = attackPoint != null ? attackPoint.position : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}