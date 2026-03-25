using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform spawnTop, spawnBottom, spawnLeft, spawnRight;
    public Transform spawnTopRight, spawnTopLeft, spawnBottomRight, spawnBottomLeft;

    // =========================
    // SPAWN WAVE (NEW)
    // =========================
    public void SpawnWave(WaveConfig wave)
    {
        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo.enemyPrefab);
            }
        }
    }

    // =========================
    // SPAWN SINGLE ENEMY
    // =========================
    private void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPosition = SelectRandomPosition();

        GameObject enemyObj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // 🔥 QUAN TRỌNG: đăng ký enemy cho GameManager
        EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            GameManager.Instance.RegisterEnemy(enemyHealth);

            if (enemyHealth.isBoss)
            {
                BossHealthBarUI.Instance.SetBoss(enemyHealth);
            }
        }
    }

    // =========================
    // RANDOM SPAWN POSITION
    // =========================
    private Vector3 SelectRandomPosition()
    {
        Transform selectedTransform = null;

        int randomValue = Random.Range(0, 8);
        SpawnPointType spawnType = (SpawnPointType)randomValue;

        switch (spawnType)
        {
            case SpawnPointType.TopLeft: selectedTransform = spawnTopLeft; break;
            case SpawnPointType.TopRight: selectedTransform = spawnTopRight; break;
            case SpawnPointType.BottomLeft: selectedTransform = spawnBottomLeft; break;
            case SpawnPointType.BottomRight: selectedTransform = spawnBottomRight; break;
            case SpawnPointType.Top: selectedTransform = spawnTop; break;
            case SpawnPointType.Bottom: selectedTransform = spawnBottom; break;
            case SpawnPointType.Right: selectedTransform = spawnRight; break;
            case SpawnPointType.Left: selectedTransform = spawnLeft; break;
        }

        if (selectedTransform == null)
        {
            Debug.LogError("Spawn point not assigned!");
            return Vector3.zero;
        }

        return selectedTransform.position + (Vector3)Random.insideUnitCircle * 0.6f;
    }
}

public enum SpawnPointType
{
    TopLeft, TopRight, BottomLeft, BottomRight, Top, Bottom, Right, Left
}