using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public float smoothSpeed = 5f;
    public CompositeCollider2D mapBounds;

    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        Vector3 smoothPos = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        if (mapBounds != null)
        {
            Bounds bounds = mapBounds.bounds;

            float camHeight = Camera.main.orthographicSize;
            float camWidth = camHeight * Screen.width / Screen.height;

            float minX = bounds.min.x + camWidth;
            float maxX = bounds.max.x - camWidth;

            float minY = bounds.min.y + camHeight;
            float maxY = bounds.max.y - camHeight;

            smoothPos.x = Mathf.Clamp(smoothPos.x, minX, maxX);
            smoothPos.y = Mathf.Clamp(smoothPos.y, minY, maxY);
        }

        transform.position = smoothPos;
    }
}