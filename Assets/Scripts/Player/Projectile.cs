using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;

    public int pierceCount = 0;
    public int ricochetCount = 0;

    public float ricochetRange = 6f;

    public int maxPierce = 3;
    public int maxRicochet = 2;

    Vector2 direction;

    List<Transform> hitEnemies = new List<Transform>();

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;

        hitEnemies.Clear();

        pierceCount = Mathf.Clamp(pierceCount, 0, maxPierce);
        ricochetCount = Mathf.Clamp(ricochetCount, 0, maxRicochet);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        if (!other.CompareTag("Enemy"))
            return;

        Transform enemy = other.transform;

        // tránh hit lại enemy cũ
        if (hitEnemies.Contains(enemy))
            return;

        hitEnemies.Add(enemy);

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // RICOCHET LOGIC
        if (ricochetCount > 0)
        {
            Transform nextTarget = FindNextEnemy(enemy);

            if (nextTarget != null)
            {
                direction =
                    (nextTarget.position - transform.position).normalized;

                ricochetCount--;
                return;
            }
        }

        // Pierce logic
        if (pierceCount > 0)
        {
            pierceCount--;
            return;
        }

        Destroy(gameObject);
    }

    Transform FindNextEnemy(Transform currentEnemy)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, ricochetRange);

        float closest = Mathf.Infinity;
        Transform target = null;

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            Transform enemy = hit.transform;

            if (enemy == currentEnemy)
                continue;

            if (hitEnemies.Contains(enemy))
                continue;

            float dist =
                Vector2.Distance(transform.position, enemy.position);

            if (dist < closest)
            {
                closest = dist;
                target = enemy;
            }
        }

        return target;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
