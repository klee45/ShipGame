using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour
{
    [SerializeField]
    private int widthCount = 1;
    [SerializeField]
    private int heightCount = 1;
    [SerializeField]
    private int widthOffset = 0;
    [SerializeField]
    private int heightOffset = 0;
    [SerializeField]
    private int widthSize = 0;
    [SerializeField]
    private int heightSize = 0;
    [SerializeField]
    private int widthDistance = 0;
    [SerializeField]
    private int heightDistance = 0;

    [SerializeField]
    private GameObject iconPrefab;

    [SerializeField]
    private WeaponIcon[] icons;

    private void Awake()
    {
        Setup(widthCount, heightCount);
    }

    private void Reset()
    {
    }

    public void Setup(int widthCount, int heightCount)
    {
        icons = new WeaponIcon[widthCount * heightCount];
        int i = 0;
        for (int y = 0; y < heightCount; y++)
        {
            for (int x = widthCount - 1; x >= 0; x--)
            {
                GameObject obj = Instantiate(iconPrefab);
                obj.transform.SetParent(transform);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(
                    -((x * (widthSize + widthDistance)) + widthOffset),
                    -((y * (heightSize + heightDistance)) + heightOffset));
                //Debug.Log(string.Format("{0} {1}", obj.transform.localPosition.x, obj.transform.localPosition.y));
                WeaponIcon icon = obj.GetComponent<WeaponIcon>();
                obj.transform.localScale = new Vector3(widthSize, heightSize);
                icons[i++] = icon;
                obj.SetActive(false);
            }
        }
    }

    public void SetIcon(int num, AWeapon weapon)
    {
        icons[num].gameObject.SetActive(true);
        icons[num].SetIcon(weapon);
    }

    public void RemoveIcon(int num)
    {
        icons[num].gameObject.SetActive(false);
    }

    public void SetPercent(int num, AWeapon weapon)
    {
        float percent;
        if (weapon.IsReady())
        {
            percent = 0;
        }
        else
        {
            percent = weapon.GetCooldownTimer().GetPercentLeft();
        }
        icons[num].UpdatePercent(percent);
    }
}
