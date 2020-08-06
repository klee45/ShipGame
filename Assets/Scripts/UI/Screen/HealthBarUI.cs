using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private static float SCALE = 0.2f;

    [SerializeField]
    private Image back;
    [SerializeField]
    private Image front;
    [SerializeField]
    private Text text;

    public void Activate()
    {
        back.enabled = true;
        front.enabled = true;
        text.enabled = true;
    }

    public void Deactivate()
    {
        back.enabled = false;
        front.enabled = false;
        text.enabled = false;
    }

    public void SetPercent(int max, int current)
    {
        //Debug.Log(string.Format("Percent is now {0}", (float)current / (float)max));
        front.fillAmount = (float)current / (float)max;
        text.text = string.Format("{0}", current);
    }

    public void SetColor(Color color)
    {
        back.color = new Color(
            color.r * SCALE,
            color.g * SCALE,
            color.b * SCALE);
        front.color = color;
    }
}
