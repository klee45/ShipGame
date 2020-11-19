﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDeed : Deed<AWeapon, Ship>
{
    [System.Serializable]
    public struct WeaponDeedInfo
    {
        public Size weaponSize;
        public int rarity;
        public int price;
    }

    [SerializeField]
    private WeaponDeedInfo[] sizeRarityPairs;
    [SerializeField]
    private AWeapon weapon;

    [SerializeField]
    private Size weaponSize;

    [SerializeField]
    private int rarity;

    private WeaponRarity rarityType;

    private static int[] rarityRanges = new int[] { 10, 25, 45, 70, 100 };

    public void Setup(WeaponDeedInfo info)
    {
        this.weaponSize = info.weaponSize;
        this.rarity = info.rarity;
        this.price = info.price;

        int pos = 0;
        foreach (int rarityCheck in rarityRanges)
        {
            if (rarity <= rarityCheck)
            {
                break;
            }
            pos++;
        }

        rarityType = (WeaponRarity)pos;

        weapon = Instantiate(weapon);
        weapon.transform.SetParent(transform);
        weapon.SetupSlotSizeMods(this.weaponSize);

        /*
        foreach (SizeMod sizeMod in GetComponentsInChildren<SizeMod>())
        {
            sizeMod.SetupSlot(weaponSize);
        }
        foreach (DescriptionSwitch descriptionMod in GetComponentsInChildren<DescriptionSwitch>())
        {
            descriptionMod.Setup(weaponSize);
        }
        */
    }

    public WeaponDeedInfo[] GetSizeRarityPairs()
    {
        return sizeRarityPairs;
    }

    public bool TryLink(Ship ship)
    {
        return ship.GetArsenal().TrySetWeapon(weapon);
    }

    public override AWeapon Create(Ship ship)
    {
        return weapon;
        /*
        weapon.Setup(
            icon, energy, cooldownTime,
            weaponSize, combatType, rarity,
            preferedPosition);
        */
    }

    public bool IsSame(WeaponDeed deed)
    {
        return
            deed.rarityType == rarityType &&
            deed.weaponSize == weaponSize;
    }

    public Size GetSize() { return weaponSize; }
    public WeaponRarity GetRarityType() { return rarityType; }
    public int GetRarity() { return rarity; }
    public string GetDescription() { return weapon.GetDescription(); }
    public string GetDamageString() { return weapon.GetDamageString(); }
    public int GetEnergyCost() { return weapon.GetEnergyCost(); }
    public float GetCooldown() { return weapon.GetCooldown(); }
    public Sprite GetIcon() { return weapon.GetIcon(); }

    public string GetName()
    {
        return weapon.GetName() + " " + GetSize().ToString()[0];
    }

    public enum WeaponRarity
    {
        Legendary,
        Epic,
        Rare,
        Uncommon,
        Common,
    }
}
