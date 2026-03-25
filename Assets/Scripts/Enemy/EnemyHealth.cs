using UnityEngine;

public class EnemyHealth : Health
{
    private EnemyHealthBar healthBar;
    public bool isBoss = false;

    protected override void Awake()
    {
        base.Awake();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemy(this);
        }
            

        if (!isBoss)
        {
            healthBar = GetComponentInChildren<EnemyHealthBar>(true);
            if (healthBar != null)
                healthBar.Init(this);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (healthBar != null)
        {
            healthBar.Show();
        }
    }

    protected override void Die()
    {
        base.Die();

        GetComponent<EnemyDrop>()?.Drop();

        Destroy(gameObject);     
    }
}