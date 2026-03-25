using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public EnemyController enemyController;
    public PlayerHealth playerHealth;

    [Header("Stages")]
    public LevelConfig[] stages;

    private UIDocument uiDoc;
    private VisualElement root;

    private VisualElement winPanel;
    private VisualElement losePanel;
    private Label stageFinishedLabel;
    private Label waveCountdownLabel;
    private Button continueButton;
    private Button replayButton;

    private int currentStageIndex = 0;
    private int currentWaveIndex = 0;

    private float waveTimer = 0f;
    private bool waveTriggered = false;

    private int totalSpawnedThisWave = 0;
    private int totalKilledThisWave = 0;
    private int aliveEnemies = 0;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        uiDoc = FindObjectOfType<UIDocument>();
        root = uiDoc.rootVisualElement;

        winPanel = root.Q<VisualElement>("WinPanel");
        losePanel = root.Q<VisualElement>("LosePanel");
        stageFinishedLabel = root.Q<Label>("stageFinishedLabel");
        waveCountdownLabel = root.Q<Label>("WaveCountdownLabel");
        Debug.Log("Countdown label = " + waveCountdownLabel);

        continueButton = root.Q<Button>("ContinueButton");
        replayButton = root.Q<Button>("ReplayButton");

        continueButton.clicked += NextStage;
        replayButton.clicked += RestartGame;

        winPanel.style.display = DisplayStyle.None;
        losePanel.style.display = DisplayStyle.None;
        waveCountdownLabel.style.display = DisplayStyle.None;

        playerHealth.OnDied.AddListener(OnPlayerDied);
    }

    private void Update()
    {
        if (isGameOver || waveTriggered)
            return;

        WaveConfig wave = GetCurrentWave();
        if (wave == null)
            return;

        waveTimer += Time.deltaTime;

        int killThreshold = Mathf.CeilToInt(
            totalSpawnedThisWave * wave.killThresholdPercent
        );

        if (totalSpawnedThisWave > 0 &&
            (totalKilledThisWave >= killThreshold || waveTimer >= wave.timeLimit))
        {
            TriggerNextWave(wave.nextWaveDelay);
        }
    }

    public void StartStage(int stageIndex)
    {
        Time.timeScale = 1f;
        isGameOver = false;

        currentStageIndex = stageIndex;
        currentWaveIndex = 0;

        ClearStageObjects();

        playerHealth.RestoreToMaxHP();

        StartWave();
    }

    private void ApplyWaveDefaults(WaveConfig wave)
    {
        if (wave.killThresholdPercent <= 0f)
            wave.killThresholdPercent = 0.7f;

        if (wave.timeLimit <= 0f)
            wave.timeLimit = 30f;

        if (wave.nextWaveDelay <= 0f)
            wave.nextWaveDelay = 3f;
    }

    private void StartWave()
    {
        waveTriggered = false;
        waveTimer = 0f;
        totalKilledThisWave = 0;

        WaveConfig wave = GetCurrentWave();
        ApplyWaveDefaults(wave);

        totalSpawnedThisWave = 0;
        foreach (var info in wave.enemies)
        {
            totalSpawnedThisWave += info.count;
        }

        enemyController.SpawnWave(wave);
    }

    private WaveConfig GetCurrentWave()
    {
        if (currentWaveIndex >= stages[currentStageIndex].waves.Length)
            return null;

        return stages[currentStageIndex].waves[currentWaveIndex];
    }

    private void TriggerNextWave(float delay)
    {
        if (waveTriggered) return;

        waveTriggered = true;

        if (currentWaveIndex + 1 >= stages[currentStageIndex].waves.Length)
        {
            currentWaveIndex++;
            CheckStageCompletion();
            return;
        }

        StartCoroutine(NextWaveCoroutine(delay));
    }

    private IEnumerator NextWaveCoroutine(float delay)
    {
        float timer = delay;

        waveCountdownLabel.style.display = DisplayStyle.Flex;

        while (timer > 0)
        {
            waveCountdownLabel.text = $"Next wave in {Mathf.Ceil(timer)}...";
            timer -= Time.deltaTime;
            yield return null;
        }

        waveCountdownLabel.style.display = DisplayStyle.None;

        currentWaveIndex++;
        StartWave();
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        aliveEnemies++;
        enemy.OnDied.AddListener(OnEnemyDied);
    }

    private void OnEnemyDied()
    {
        if (isGameOver) return;

        totalKilledThisWave++;
        aliveEnemies--;

        WaveConfig wave = GetCurrentWave();

        if (wave != null)
        {
            int killThreshold = Mathf.CeilToInt(
                totalSpawnedThisWave * wave.killThresholdPercent
            );

            if (totalKilledThisWave >= killThreshold)
            {
                TriggerNextWave(wave.nextWaveDelay);
            }
        }

        CheckStageCompletion();
    }

    private void CheckStageCompletion()
    {
        bool allWavesSpawned = currentWaveIndex >= stages[currentStageIndex].waves.Length;

        if (allWavesSpawned && aliveEnemies <= 0)
        {
            OnStageFinished();
        }
    }

    private void OnPlayerDied()
    {
        if (isGameOver) return;

        isGameOver = true;
        losePanel.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    private void OnStageFinished()
    {
        isGameOver = true;

        int stageNumber = currentStageIndex + 1;
        stageFinishedLabel.text = $"Stage {stageNumber} Finished!";

        winPanel.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    private void NextStage()
    {
        winPanel.style.display = DisplayStyle.None;
        Time.timeScale = 1f;

        int nextIndex = currentStageIndex + 1;
        if (nextIndex >= stages.Length)
            nextIndex = 0;

        StartStage(nextIndex);
    }

    public void RestartGame()
    {
        losePanel.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        StartStage(0);
    }

    public void ClearStageObjects()
    {
        // Xóa enemy
        foreach (var enemy in FindObjectsOfType<EnemyHealth>())
        {
            Destroy(enemy.gameObject);
        }

        // Xóa toàn bộ enemy bullets
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (var enemybullet in enemyBullets)
        {
            Destroy(enemybullet);
        }

        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        foreach (var playerBullet in playerBullets)
        {
            Destroy(playerBullet);
        }
    }
}