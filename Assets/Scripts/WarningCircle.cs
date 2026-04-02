using UnityEngine;
using UnityEngine.Rendering;

public class WarningCircle : MonoBehaviour
{
    public float duration = 1f;
    public float maxScale = 1f;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / duration;

        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxScale, t);

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float radius, float time)
    {
        maxScale = radius;
        duration = time;
    }
}
