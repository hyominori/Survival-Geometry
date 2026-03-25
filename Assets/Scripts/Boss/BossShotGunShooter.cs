using UnityEngine;
using System.Collections;

public class BossShotGunShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Transform player;

    [Header("Shotgun Settings")]
    public float attackCooldown = 3f;   // bao lâu bắn 1 lần
    public int bursts = 3;              // số lượt bắn
    public float burstDelay = 0.3f;     // delay giữa mỗi lượt

    public int bulletsPerShot = 5;      // số đạn mỗi lượt
    public float spreadAngle = 45f;     // góc tỏa
    public float bulletSpeed = 8f;

    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            StartCoroutine(FireShotgun());
            timer = 0;
        }
    }

    IEnumerator FireShotgun()
    {
        for (int b = 0; b < bursts; b++)
        {
            ShootSpread();

            yield return new WaitForSeconds(burstDelay);
        }
    }

    void ShootSpread()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        float startAngle = baseAngle - spreadAngle / 2;

        float angleStep = spreadAngle / (bulletsPerShot - 1);

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = startAngle + angleStep * i;

            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * bulletSpeed;
        }
    }
}