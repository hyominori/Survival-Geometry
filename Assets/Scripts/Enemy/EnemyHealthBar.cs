using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public float visibleTime = 1.5f;

    private Health target;
    private float timer;

    public void Init(Health health)
    {
        target = health;

        slider.maxValue = target.maxHP;
        slider.value = target.currentHP;
        slider.gameObject.SetActive(false);

        // 🔥 UnityEvent -> dùng AddListener
        target.OnHealthChanged.AddListener(HandleHealthChanged);
        target.OnDied.AddListener(HandleTargetDied);
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.OnHealthChanged.RemoveListener(HandleHealthChanged);
            target.OnDied.RemoveListener(HandleTargetDied);
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    private void HandleTargetDied()
    {
        if (slider != null)
            slider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!slider.gameObject.activeSelf) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
            slider.gameObject.SetActive(false);
    }

    public void Show()
    {
        if (slider == null) return;

        slider.gameObject.SetActive(true);
        timer = visibleTime;
    }

    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}