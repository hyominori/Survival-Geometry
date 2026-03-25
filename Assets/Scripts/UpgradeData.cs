using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public UpgradeType type;
    public float value;

    public bool requireProjectile;
    public bool requireHPRegen;
}
