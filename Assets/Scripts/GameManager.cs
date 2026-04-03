using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public EnemyController enemyController;
    public PlayerHealth playerHealth;
    private MenuUI menuUI;
    private  HashSet<EnemyHealth> registeredEnemies = new HashSet<EnemyHealth>();

    [Header("Stages")]
    public LevelConfig[] stages;

    private UIDocument uiDoc;
    private VisualElement root;

    private VisualElement winPanel;
    private VisualElement losePanel;
    private Label stageFinishedLabel;
    private Label waveCountdownLabel;
    private Button continueButton;
    private Button winReplayButton;
    private Button loseReplayButton;
    private Button winMenuButton;
    private Button loseMenuButton;

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
        menuUI = FindObjectOfType<MenuUI>();

        winPanel = root.Q<VisualElement>("WinPanel");
        losePanel = root.Q<VisualElement>("LosePanel");
        stageFinishedLabel = root.Q<Label>("stageFinishedLabel");
        waveCountdownLabel = root.Q<Label>("WaveCountdownLabel");
        Debug.Log("Countdown label = " + waveCountdownLabel);

        continueButton = root.Q<Button>("ContinueButton");
        winReplayButton = root.Q<Button>("WinReplayButton");
        winMenuButton = root.Q<Button>("WinMenuButton"); 
        loseReplayButton = root.Q<Button>("LoseReplayButton");
        loseMenuButton = root.Q<Button>("LoseMenuButton");

        continueButton.clicked += NextStage;
        winReplayButton.clicked += RestartGame;
        loseReplayButton.clicked += RestartGame;
        winMenuButton.clicked += () =>
        {
            winPanel.style.display = DisplayStyle.None;
            menuUI.BackToMenu();
        };
        loseMenuButton.clicked += () =>
        {
            losePanel.style.display = DisplayStyle.None;
            menuUI.BackToMenu();
        };

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
        if (registeredEnemies.Contains(enemy))
        {
            return;
        }
        registeredEnemies.Add(enemy);

        aliveEnemies++;
        enemy.OnDied.AddListener(OnEnemyDied);
    }

    private void OnEnemyDied()
    {
        if (isGameOver) return;

        totalKilledThisWave++;
        aliveEnemies--;

        registeredEnemies.RemoveWhere(e => e == null);

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

        bool isFinalStage = currentStageIndex >= stages.Length - 1;

        if (isFinalStage)
        {
            stageFinishedLabel.text = "Final Stage Finished!";

            // Ẩn nút Next
            continueButton.style.display = DisplayStyle.None;

            // Hiện nút Replay
            winReplayButton.style.display = DisplayStyle.Flex;

            winMenuButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            stageFinishedLabel.text = $"Stage {stageNumber} Finished!";

            continueButton.style.display = DisplayStyle.Flex;
            winReplayButton.style.display = DisplayStyle.None;
        }

        winPanel.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    private void NextStage()
    {
        winPanel.style.display = DisplayStyle.None;
        Time.timeScale = 1f;

        int nextIndex = currentStageIndex + 1;

        if (nextIndex >= stages.Length)
            return;

        StartStage(nextIndex);
    }

    public void RestartGame()
    {
        losePanel.style.display = DisplayStyle.None;
        winPanel.style.display = DisplayStyle.None;
        losePanel.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        ResetGame();
        StartStage(0);
    }

    public void ClearStageObjects()
    {
        ClearAllProjectiles();
        ClearEnemies();
    }

    public void ClearEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyHealth>())
        {
            Destroy(enemy.gameObject);
        }
    }

    public void ClearAllProjectiles()
    {
        foreach (var p in FindObjectsOfType<Projectile>())
            Destroy(p.gameObject);

        foreach (var p in FindObjectsOfType<BossBullet>())
            Destroy(p.gameObject);
    }

    public void ClearAllItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
    }

    void ResetAllSystem()
    {
        var resettables = FindObjectsOfType<MonoBehaviour>().OfType<IResettable>();
        foreach (var r in resettables)
        {
            r.ResetState();
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;

        // Reset stage
        currentStageIndex = 0;
        currentWaveIndex = 0;

        waveTriggered = false;
        waveTimer = 0f;

        totalKilledThisWave = 0;
        totalSpawnedThisWave = 0;
        aliveEnemies = 0;

        isGameOver = false;

        // Clear enemy + đạn
        ClearEnemies();
        ClearAllProjectiles();
        ClearAllItems();

        ResetAllSystem();
    }
}