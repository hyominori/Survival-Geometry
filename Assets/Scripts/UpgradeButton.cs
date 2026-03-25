using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text label;

    private UpgradeData upgrade;
    private LevelUpUI ui;

    public void Setup(UpgradeData data, LevelUpUI levelUI)
    {
        upgrade = data;
        ui = levelUI;

        label.text = data.upgradeName;
    }

    public void OnClick()
    {
        UpgradeManager.Instance.ApplyUpgrade(upgrade);
        ui.Close();
    }
}