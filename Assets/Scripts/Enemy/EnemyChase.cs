using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;

    private Rigidbody2D rb;

    private Vector2 knockbackVelocity;
    private float knockbackTimer;
    public float knockbackDuration = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // 👉 Nếu đang bị knockback → ưu tiên knockback
        if (knockbackTimer > 0)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        // 👉 Di chuyển bình thường
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;
    }

    public void ApplyKnockback(Vector2 force)
    {
        knockbackVelocity = force;
        knockbackTimer = knockbackDuration;
    }
}