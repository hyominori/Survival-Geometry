using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;
    public float lifeTime = 5f;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;

        // Bullet tự hủy sau vài giây
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
                player.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        // Wall
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }


    // Khi ra khỏi màn hình thì hủy
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}