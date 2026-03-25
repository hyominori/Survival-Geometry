using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Health target;
    public Image fill;

    void Update()
    {
        if (target == null) return;
        fill.fillAmount = target.currentHP/ target.maxHP;
    }
}
