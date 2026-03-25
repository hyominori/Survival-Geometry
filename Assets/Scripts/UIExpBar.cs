using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    public Slider expSlider;

    public TMP_Text levelText;

    public void UpdateExp(int current, int max)
    {
        expSlider.maxValue = max;
        expSlider.value = current;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "LV " + level;
    }
}
