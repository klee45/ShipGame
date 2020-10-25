﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : Singleton<ShopInterface>
{
    [SerializeField]
    private Vector2 weaponsPos;
    [SerializeField]
    private WeaponButton buttonPrefab;
    [SerializeField]
    private ButtonRow[] rows;
    [SerializeField]
    private Text moneyText;

    [SerializeField]
    private GameObject visual;

    private List<WeaponButton> allWeaponButtons;

    protected override void Awake()
    {
        base.Awake();
        allWeaponButtons = new List<WeaponButton>();
        visual.SetActive(false);
    }

    public void SetupShop()
    {
        foreach (ButtonRow row in rows)
        {
            allWeaponButtons.AddRange(row.SetupButtons(buttonPrefab));
        }

        foreach (WeaponButton button in allWeaponButtons)
        {
            WeaponDeed deed = Instantiate(DropTable.instance.GetRandomWeaponDeed());
            deed.transform.SetParent(transform);
            button.SetWeaponDeed(deed);
        }
    }

    public void OpenShop()
    {
        visual.SetActive(true);
        PlayerInfo.instance.GetBank().OnMoneyChange += UpdateMoneyVisual;
        UpdateMoneyVisual(0);
    }

    public void CloseShop ()
    {
        PlayerInfo.instance.GetBank().OnMoneyChange -= UpdateMoneyVisual;
        visual.SetActive(false);
    }

    private void UpdateMoneyVisual(int change)
    {
        moneyText.text = PlayerInfo.instance.GetBank().GetTotalMoney().ToString();
    }
}
