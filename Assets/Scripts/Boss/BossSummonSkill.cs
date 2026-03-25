using UnityEngine;

public class BossSummonSkill : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int summonCount = 4;
    public float summonRadius = 3f;

    public float cooldown = 10f;
    private float cooldownTimer;

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= cooldown)
        {
            SummonEnemies();
            cooldownTimer = 0f;
        }
    }

    void SummonEnemies()
    {
        for (int i = 0; i < summonCount; i++)
        {
            float angle = i * Mathf.PI * 2 / summonCount;

            Vector2 spawnPos = (Vector2)transform.position +
                               new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * summonRadius;

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}