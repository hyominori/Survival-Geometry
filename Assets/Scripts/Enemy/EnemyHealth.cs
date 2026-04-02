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
        
        var chase = GetComponent<EnemyChase>();
        if (chase != null)
        {
            chase.enabled = false;
        }

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }   

        var explode = GetComponent<EnemyExplodeOnDeath>();

        if (explode != null) 
        {
            explode.Explode();
        }
        else
        {
            Destroy(gameObject);
        }    
        
        GetComponent<EnemyDrop>()?.Drop();
    }
}