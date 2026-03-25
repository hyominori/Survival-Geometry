using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float damage = 10f;
    public float knockbackForce = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            EnemyChase move = other.GetComponent<EnemyChase>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (move != null && !enemy.isBoss)
                {
                    Vector2 dir = (other.transform.position - transform.position).normalized;
                    move.ApplyKnockback(dir * knockbackForce);
                }
            }
        }
    }
}
