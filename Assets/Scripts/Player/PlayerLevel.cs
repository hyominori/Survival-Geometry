using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;

    public int currentExp = 0;
    public int expToNextLevel = 10;

    public int expGrowth = 5; // mỗi level tăng thêm exp cần

    public UIExpBar expBar;

    public LevelUpUI levelUpUI;
    private void Start()
    {
        UpdateUI();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        level++;

        currentExp -= expToNextLevel;

        expToNextLevel += expGrowth;

        Debug.Log("Level Up! Level: " + level);

        levelUpUI.ShowLevelUp();
    }

    void UpdateUI()
    {
        if (expBar != null)
        {
            expBar.UpdateExp(currentExp, expToNextLevel);
            expBar.UpdateLevel(level);
        }
    }
}