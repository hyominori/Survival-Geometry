using System;
using UnityEngine;

public class PlayerHealth : Health, IResettable
{
    public float invincibleTime = 1.5f;
    private bool isInvincible;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashInterval = 0.1f;

    public bool IsInvincible => isInvincible;

    public void ResetState()
    {
        RestoreToMaxHP();
        maxHP = 100;
    }

    public override void TakeDamage(float damage)
    {
        if (isInvincible) return;

        base.TakeDamage(damage);
        StartCoroutine(InvincibleCoroutine());
    }

    private System.Collections.IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibleTime)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashInterval);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashInterval);

            timer += flashInterval * 2;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;

    }

    public void Heal(float amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Player died");
    }
}
