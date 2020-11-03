﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    [SerializeField]
    private Text name, description, resource, size, cooldown, rarity, damage;
    [SerializeField]
    private Image icon;

    private bool changed = false;

    private void Start()
    {
        ResetImage();
    }

    public void ShowDescription(WeaponDeed deed)
    {
        changed = true;
        name.text = deed.GetName();
        description.text = deed.GetDescription();
        SetResource(deed.GetEnergyCost());
        size.text = "Size\n" + deed.GetSize().ToString();
        cooldown.text = "Cooldown\n" + deed.GetCooldown();
        SetRarity(deed.GetRarityType(), deed.GetRarity());
        damage.text = deed.GetDamageString();
        icon.sprite = deed.GetIcon();
        icon.color = Color.white;
    }

    public void ResetImage()
    {
        name.text = "";
        description.text = "";
        resource.text = "";
        size.text = "";
        cooldown.text = "";
        rarity.text = "";
        damage.text = "";
        icon.color = Color.clear;
    }

    private void SetResource(int energyCost)
    {
        if (energyCost > 0)
        {
            resource.text = "Energy\n" + energyCost;
        }
        else
        {
            resource.text = "Ammo";
        }
    }

    private void SetRarity(WeaponDeed.WeaponRarity rarityType, int rarityValue)
    {
        Color color = Colors.rarityColors[(int)rarityType];
        rarity.text = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + rarityType.ToString() + "</color>\n" + rarityValue;
    }

}