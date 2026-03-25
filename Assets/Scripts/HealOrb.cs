using UnityEngine;

public class HealOrb : MonoBehaviour
{
    public float healAmount = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.Heal(healAmount);
        }

        Destroy(gameObject);
    }
}