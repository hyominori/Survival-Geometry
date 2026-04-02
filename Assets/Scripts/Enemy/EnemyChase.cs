using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;
    private EnemyHealth health;
    private Rigidbody2D rb;

    private Vector2 knockbackVelocity;
    private float knockbackTimer;
    public float knockbackDuration = 0.2f;
    private float knockbackCooldown = 0.5f;
    private float lastKnockbackTime = -999f;
    public float knockbackResistance = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();   
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // 👉 Nếu đang bị knockback → ưu tiên knockback
        if (knockbackTimer > 0 && knockbackVelocity != Vector2.zero)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        if (health != null && health.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 👉 Di chuyển bình thường
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (force.magnitude < 0.01f)
        {
            return;
        }

        if (Time.time < lastKnockbackTime + knockbackCooldown)
        {
            return;
        }

        lastKnockbackTime = Time.time;

        Vector2 finalForce = force / knockbackResistance;

        knockbackVelocity = finalForce;
        knockbackTimer = knockbackDuration;
    }
}