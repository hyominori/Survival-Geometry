using UnityEngine;

public class BossRadialShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount = 12;
    public float cooldown = 3f;
    public float bulletSpeed = 5f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            ShootRadial();
            timer = 0f;
        }
    }

    void ShootRadial()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * bulletSpeed;
        }
    }
}