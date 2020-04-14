﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour
{
    [SerializeField]
    private int widthCount;
    [SerializeField]
    private int heightCount;
    [SerializeField]
    private int widthOffset;
    [SerializeField]
    private int heightOffset;
    [SerializeField]
    private int widthSize;
    [SerializeField]
    private int heightSize;

    [SerializeField]
    private GameObject iconPrefab;

    [SerializeField]
    private WeaponIcon[] icons;

    private static WeaponsUI instance;

    public static WeaponsUI Instance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Weapons ui is singleton");
            Destroy(instance);
        }
        instance = this;
        Setup(widthCount, heightCount);
    }

    private void Reset()
    {
        foreach (WeaponIcon obj in GetComponentsInChildren<WeaponIcon>())
        {
            Destroy(obj);
        }
    }

    public void Setup(int widthCount, int heightCount)
    {
        Reset();

        icons = new WeaponIcon[widthCount * heightCount];
        int i = 0;
        for (int y = 0; y < heightCount; y++)
        {
            for (int x = widthCount - 1; x >= 0; x--)
            {
                GameObject obj = Instantiate(iconPrefab);
                Vector3 scale = obj.transform.localScale;
                obj.transform.SetParent(transform);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(
                    -((x * widthSize) + widthOffset),
                    -((y * heightSize) + heightOffset));
                //Debug.Log(string.Format("{0} {1}", obj.transform.localPosition.x, obj.transform.localPosition.y));
                WeaponIcon icon = obj.GetComponent<WeaponIcon>();
                obj.transform.localScale = scale;
                icons[i++] = icon;
                obj.SetActive(false);
            }
        }
    }

    public void SetIcon(int num, Weapon weapon)
    {
        icons[num].SetIcon(weapon.GetIcon());
        icons[num].gameObject.SetActive(true);
    }

    public void SetPercent(int num, Weapon weapon)
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