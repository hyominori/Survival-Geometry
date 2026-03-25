using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public UpgradeButton[] buttons;

    public UpgradeData[] upgradePool;

    public void ShowLevelUp()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        List<UpgradeData> validUpgrades = GetValidUpgrades();

        List<UpgradeData> selected = new List<UpgradeData>();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (validUpgrades.Count == 0) break;

            int index = Random.Range(0, validUpgrades.Count);
            UpgradeData upgrade = validUpgrades[index];

            selected.Add(upgrade);
            validUpgrades.RemoveAt(index);

            buttons[i].Setup(upgrade, this);
        }
    }

    List<UpgradeData> GetValidUpgrades()
    {
        List<UpgradeData> valid = new List<UpgradeData>();

        foreach (UpgradeData up in upgradePool)
        {
            if (IsUpgradeUnlocked(up))
            {
                valid.Add(up);
            }
        }

        return valid;
    }

    bool IsUpgradeUnlocked(UpgradeData upgrade)
    {
        var manager = UpgradeManager.Instance;

        switch (upgrade.type)
        {
            // 🔥 phụ thuộc projectile
            case UpgradeType.ProjectileMultiShot:
            case UpgradeType.ProjectilePierce:
            case UpgradeType.ProjectileRicochet:
                if (!manager.hasProjectile)
                    return false;
                break;

            // 🔥 phụ thuộc HP regen
            case UpgradeType.HPRegenCooldown:
                if (!manager.hasHPRegen)
                    return false;
                break;
        }

        return true;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}