using UnityEngine;

public class BossSpiralShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Header("Spiral Settings")]
    public float attackDuration = 4f;     // bắn trong bao lâu
    public float attackCooldown = 3f;     // bao lâu mới bắn lại
    public float fireInterval = 0.1f;     // thời gian giữa mỗi viên
    public float spiralSpeed = 10f;
    public float bulletSpeed = 6f;

    float fireTimer = 0f;
    float attackTimer = 0f;
    float cooldownTimer = 0f;

    float angle = 0f;

    bool isAttacking = false;

    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireInterval)
            {
                Fire();
                fireTimer = 0f;
            }

            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                attackTimer = 0f;
                cooldownTimer = 0f;
            }
        }
        else
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= attackCooldown)
            {
                StartAttack();
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        attackTimer = 0f;
        fireTimer = 0f;
    }

    void Fire()
    {
        angle += spiralSpeed;

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