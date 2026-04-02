using UnityEngine;
using System.Collections;

public class EnemyExplodeOnDeath : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 2f;
    public float damage = 10f;
    public float delay = 0.3f;

    [Header("References")]
    public GameObject warningCirclePrefab;
    public LayerMask playerLayer;

    private bool exploded = false;

    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Explode()
    {
        if (exploded) return;
        exploded = true;

        // 👉 tắt collider để tránh bug va chạm
        if (col != null)
            col.enabled = false;

        StartCoroutine(ExplodeCoroutine());
    }

    IEnumerator ExplodeCoroutine()
    {
        Debug.Log("💥 EXPLOSION START");

        Vector2 center = GetExplosionCenter();

        // 👉 spawn warning circle
        if (warningCirclePrefab != null)
        {
            GameObject circle = Instantiate(
                warningCirclePrefab,
                center,
                Quaternion.identity
            );

            var wc = circle.GetComponent<WarningCircle>();
            if (wc != null)
                wc.Init(explosionRadius, delay);
        }

        // 👉 blink effect
        float timer = 0f;
        while (timer < delay)
        {
            if (sr != null)
                sr.enabled = !sr.enabled;

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        if (sr != null)
            sr.enabled = true;

        // 👉 DAMAGE PHASE
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            center,
            explosionRadius
        );

        Debug.Log("💥 Total hits: " + hits.Length);

        foreach (var hit in hits)
        {
            PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();

            if (player != null)
            {
                float dist = Vector2.Distance(center, player.transform.position);

                // 👉 damage falloff (optional nhưng rất nên dùng)
                float finalDamage = damage * (1 - dist / explosionRadius);
                finalDamage = Mathf.Clamp(finalDamage, 1f, damage);

                Debug.Log($"🔥 Hit Player - Damage: {finalDamage}");

                player.TakeDamage(finalDamage);
            }
        }

        // 👉 destroy sau khi xử lý xong
        Destroy(gameObject);
    }

    Vector2 GetExplosionCenter()
    {
        // 👉 dùng collider center cho chính xác
        if (col != null)
            return col.bounds.center;

        return transform.position;
    }

    // 👉 debug vùng nổ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}