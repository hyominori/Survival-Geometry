using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float pickupRange = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public float minX = -10f, maxX = 10f, minY = -5f, maxY = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;
    }   
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed; // Fix: Corrected property name from linearVelocity to velocity

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
