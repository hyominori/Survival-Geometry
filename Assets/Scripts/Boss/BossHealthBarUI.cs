using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public static BossHealthBarUI Instance;

    public Slider slider;
    private EnemyHealth bossHealth;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void SetBoss(EnemyHealth boss)
    {
        bossHealth = boss;

        slider.maxValue = bossHealth.maxHP;
        slider.value = bossHealth.currentHP;

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (bossHealth == null)
        {
            slider.value = 0;
            gameObject.SetActive(false);
            return;
        }

        slider.value = bossHealth.currentHP;
    }
}