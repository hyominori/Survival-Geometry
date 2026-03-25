using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    public float damage = 10f;
    public float damageInterval = 0.5f;

    private PlayerHealth player;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                DamageOnce();
                damageCoroutine = StartCoroutine(DamageLoop());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
            player = null;
        }
    }

    void DamageOnce()
    {
        if (!player.IsInvincible)
            player.TakeDamage(damage);
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            if (player == null)
                yield break;

            DamageOnce();
        }
    }
}
