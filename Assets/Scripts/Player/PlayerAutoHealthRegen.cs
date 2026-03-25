using System.Threading;
using UnityEngine;

public class PlayerAutoHealthRegen : MonoBehaviour
{
    public float regenAmount = 0f;
    public float cooldown = 5f;

    float timer;

    PlayerHealth health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<PlayerHealth>();  
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            health.Heal(regenAmount);
            timer = 0f;
        }
    }
}
