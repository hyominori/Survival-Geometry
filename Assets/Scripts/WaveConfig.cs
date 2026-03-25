using System;
using UnityEngine;

[Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
}

[Serializable]
public class WaveConfig
{
    [Header("Spawn Settings")]
    public EnemySpawnInfo[] enemies;

    [Header("Hybrid Trigger")]
    [Range(0f, 1f)]
    public float killThresholdPercent = 0.7f;

    public float timeLimit = 30f;

    [Header("Delay Before Next Wave")]
    public float nextWaveDelay = 3f;
}