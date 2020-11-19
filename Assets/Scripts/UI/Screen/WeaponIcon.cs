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
    [SerializeField]
    private Image border;

    public void SetIcon(AWeapon weapon)
    {
        iconFront.sprite = weapon.GetIcon();
        iconBack.sprite = weapon.GetIcon();
        border.sprite = DropTable.instance.GetBorder(weapon.GetSize());
        /*
        iconFront.type = Image.Type.Filled;
        iconFront.fillMethod = Image.FillMethod.Vertical;
        iconFront.color = new Color(0.1f, 0.1f, 0.1f);
        */
    }

    public void UpdatePercent(float percent)
    {
        iconFront.fillAmount = percent;
    }
}
