﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonShop : MonoBehaviour
{
    [SerializeField]
    private Text weaponName;
    [SerializeField]
    private Text weaponCost;
    [SerializeField]
    private Image weaponSprite;

    private bool canBuy;
    private WeaponDeed linkedDeed;

    private bool isShown = false;

    public void Click()
    {
        if (canBuy)
        {
            if (isShown)
            {
                ShopInterface.instance.GetPurchaseButton().Purchase();
            }
            else
            {
                ShopInterface.instance.GetDescriptionBox().ShowDescription(linkedDeed);
                ShopInterface.instance.GetPurchaseButton().SetLinkedButton(this);
            }
        }
        else
        {
            ShopInterface.instance.GetPurchaseButton().Clear();
        }
    }

    public void SetShown(bool val)
    {
        isShown = val;
    }

    public WeaponDeed GetLinkedDeed()
    {
        return linkedDeed;
    }

    public void CloseSale()
    {
        isShown = false;
        this.canBuy = false;
        this.linkedDeed = null;
        this.weaponName.text = "";
        this.weaponSprite.sprite = null;
        this.weaponSprite.color = Color.clear;
        this.weaponCost.text = "";
    }

    public void SetWeaponDeed(WeaponDeed deed)
    {
        this.canBuy = true;
        this.linkedDeed = deed;
        this.weaponName.text = deed.GetName();
        this.weaponSprite.sprite = deed.GetIcon();
        this.weaponSprite.color = Color.white;
        this.weaponCost.text = deed.GetPrice().ToString();
    }
}