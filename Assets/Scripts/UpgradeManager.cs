using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public PlayerMovement playerMovement;
    public WeaponController weaponOrbit;
    public PlayerHealth playerHealth;
    public PlayerProjectileShooter projectileShooter;
    public PlayerAutoHealthRegen regen;
    public float expMagnetRange = 2f;

    public bool hasProjectile = false;
    public bool hasHPRegen = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.WeaponDamage:
                weaponOrbit.IncreaseDamage(upgrade.value);
                break;

            case UpgradeType.WeaponCount:
                weaponOrbit.weaponCount += (int)upgrade.value;
                weaponOrbit.AddWeapon();
                break;

            case UpgradeType.WeaponRadius:
                weaponOrbit.radius += upgrade.value;
                break;

            case UpgradeType.OrbitSpeed:
                weaponOrbit.rotationSpeed += upgrade.value;
                break;

            case UpgradeType.MoveSpeed:
                playerMovement.moveSpeed += upgrade.value;
                break;

            case UpgradeType.MaxHP:
                playerHealth.maxHP += upgrade.value;
                playerHealth.Heal(upgrade.value);
                break;

            case UpgradeType.Heal:
                playerHealth.Heal(upgrade.value);
                break;

            case UpgradeType.AttackKnockback:
                weaponOrbit.IncreaseKnockback(upgrade.value);
                break;

            case UpgradeType.ProjectileWeapon:
                hasProjectile = true;
                projectileShooter.enabled = true;
                projectileShooter.burstCount += (int)upgrade.value;
                break;

            case UpgradeType.ProjectileMultiShot:
                projectileShooter.laneCount += (int)upgrade.value;
                break;

            case UpgradeType.ProjectilePierce:
                projectileShooter.projectilePierce += (int)upgrade.value;
                break;

            case UpgradeType.ProjectileRicochet:
                projectileShooter.projectileRicochet += (int)upgrade.value;
                break;

            case UpgradeType.HPRegen:
                hasHPRegen = true;
                regen.regenAmount += upgrade.value;
                break;

            case UpgradeType.HPRegenCooldown:
                regen.cooldown -= upgrade.value;
                break;

            case UpgradeType.Magnet:
                playerMovement.pickupRange += upgrade.value;
                break;
        }
    }
}
