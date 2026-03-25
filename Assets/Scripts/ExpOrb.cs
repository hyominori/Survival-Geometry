using System.Threading;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    int expAmount;
    //public float magnetRange = 2f;
    //public float moveSpeed = 5f;

    //Transform player;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player").transform;
    //}

    public void Init(int exp)
    {
        expAmount = exp;
    }

    //void Update()
    //{
    //    float dít = Vector2.Distance(transform.position, player.position);

    //    if (dít < magnetRange)
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed + Time.deltaTime);
    //    }
    //}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerLevel player = other.GetComponent<PlayerLevel>();

        if (player != null)
        {
            player.AddExp(expAmount);
        }

        Destroy(gameObject);
    }
}