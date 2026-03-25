using UnityEngine;

public class PlayerProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float cooldown = 2f;

    // số viên đạn bắn liên tiếp
    public int burstCount = 0;

    // số lane bắn (1 = thẳng, 3 = multishot)
    public int laneCount = 1;

    public int projectilePierce = 0;

    public int projectileRicochet = 0;

    public float laneSpread = 10f;
    public float burstDelay = 0.08f;

    float timer;


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            StartCoroutine(ShootBurst());
            timer = 0f;
        }
    }

    System.Collections.IEnumerator ShootBurst()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - transform.position).normalized;


        if (dir == Vector2.zero)
            dir = Vector2.right;

        for (int b = 0; b < burstCount; b++)
        {
            ShootSpread(dir);
            yield return new WaitForSeconds(burstDelay);
        }
    }

    void ShootSpread(Vector2 dir)
    {
        for (int i = 0; i < laneCount; i++)
        {
            float angle = laneSpread * (i - (laneCount - 1) / 2f);

            Vector2 shootDir =
                Quaternion.Euler(0, 0, angle) * dir;

            GameObject p =
    Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Projectile proj = p.GetComponent<Projectile>();

            proj.Init(shootDir);

            // truyền upgrade vào projectile
            proj.pierceCount = projectilePierce;
            proj.ricochetCount = projectileRicochet;
        }
    }
}