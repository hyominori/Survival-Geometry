using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    public float moveSpeed = 10f;

    Transform player;
    PlayerMovement playerMove;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMove = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float range = playerMove.pickupRange;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < range)
        {
            Vector2 dir =
                (player.position - transform.position).normalized;

            transform.position +=
                (Vector3)(dir * moveSpeed * Time.deltaTime);
        }
    }
}