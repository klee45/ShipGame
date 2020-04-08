using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIcon : MonoBehaviour
{
    [SerializeField]
    private Image iconFront;
    [SerializeField]
    private Image iconBack;

    public void SetIcon(Sprite sprite)
    {
        iconFront.sprite = sprite;
        /*
        iconFront.type = Image.Type.Filled;
        iconFront.fillMethod = Image.FillMethod.Vertical;
        iconFront.color = new Color(0.1f, 0.1f, 0.1f);
        */
        iconBack.sprite = sprite;
    }

    public void UpdatePercent(float percent)
    {
        iconFront.fillAmount = percent;
    }
}
