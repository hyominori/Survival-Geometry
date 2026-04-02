using System.Collections;
using UnityEngine;

public class EnemySuicideBomber : MonoBehaviour
{
    public float triggerDistance = 3f;
    public float rushSpeed = 8f;

    private Transform player;
    private Rigidbody2D rb;

    private bool isRushing = false;

    private EnemyExplodeOnDeath explode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        explode = GetComponent<EnemyExplodeOnDeath>();
    }

    private void FixedUpdate()
    {
        if (isRushing)
        {
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= triggerDistance)
        {
            StartCoroutine(RushAndExplode());
        }
    }

    IEnumerator RushAndExplode()
    {
        isRushing = true;

        // hướng lao vào
        Vector2 dir = (player.position - transform.position).normalized;

        float rushTime = 0.3f;
        float timer = 0;

        while (timer < rushTime)
        {
            rb.linearVelocity = dir * rushSpeed;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;

        explode.Explode();
    }
}
