using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public float smoothSpeed = 5f;

    public Vector3 offset = new Vector3(0, 0, -10);

    public float minX = -10f, maxX = 10f, minY = -5f, maxY = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        Vector3 smoothPos = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // 👉 clamp camera
        smoothPos.x = Mathf.Clamp(smoothPos.x, minX, maxX);
        smoothPos.y = Mathf.Clamp(smoothPos.y, minY, maxY);

        transform.position = smoothPos;
    }
}