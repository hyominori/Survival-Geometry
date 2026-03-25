using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform player;
    public GameObject weaponPrefab;

    public int weaponCount = 1;
    public float radius = 0.8f;
    public float rotationSpeed = 180f;
    public float weaponDamage = 25f;

    private List<GameObject> weapons = new List<GameObject>();

    private float currentAngle;

    void Start()
    {
        for (int i = 0; i < weaponCount; i++)
        {
            AddWeapon();
        }
    }

    public void IncreaseDamage(float amount)
    {
        weaponDamage += amount;

        foreach (GameObject w in weapons)
        {
            w.GetComponent<WeaponDamage>().damage += amount;
        }
    }

    public void AddWeapon()
    {
        GameObject w = Instantiate(weaponPrefab, player);

        WeaponDamage damage = w.GetComponent<WeaponDamage>();
        damage.damage = weaponDamage;

        weapons.Add(w);
    }

    public void IncreaseKnockback(float value)
    {
        foreach (GameObject w in weapons)
        {
            WeaponDamage dmg = w.GetComponent<WeaponDamage>();
            dmg.knockbackForce += value;
        }
    }

    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;

        for (int i = 0; i < weapons.Count; i++)
        {
            float angle = currentAngle + i * (360f / weapons.Count);
            SetWeaponPosition(weapons[i], angle);
        }
    }

    void SetWeaponPosition(GameObject weapon, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        Vector2 offset =
            new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

        weapon.transform.localPosition = offset;

        Vector2 direction = offset.normalized;

        float rotationZ =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weapon.transform.rotation =
            Quaternion.Euler(0, 0, rotationZ - 90f);
    }
}