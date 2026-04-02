using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    public float maxHP = 100;
    public float currentHP { get; protected set; }

    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnDied;

    protected bool isDead;

    public bool IsDead => isDead;

    protected virtual void Awake()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void RestoreToMaxHP()
    {
        isDead = false;               
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    protected virtual void Die()
    {
        isDead = true;
        OnDied?.Invoke();
    }

    public virtual void ResetHP()
    {
        isDead = false;
        currentHP = maxHP;
        OnHealthChanged.Invoke(currentHP, maxHP);
    }
}