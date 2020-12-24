using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDeed : Deed<AWeapon, Ship>
{
    [SerializeField]
    private int rarity;

    [SerializeField]
    private Size weaponSize;

    [System.Serializable]
    public struct WeaponDeedInfo
    {
        public Size weaponSize;
        public int rarity;
        public int price;
    }

    [Header("Deed possibilities")]
    [SerializeField]
    private WeaponDeedInfo[] sizeRarityPairs;
    [SerializeField]
    private AWeapon weapon;

    private WeaponRarity rarityType;

    private static int[] rarityRanges = new int[] { 10, 25, 45, 70, 100 };

    public void SetupFromExisting()
    {
        if (weapon.gameObject.IsPrefab())
        {
            weapon = Instantiate(weapon);
        }
        weapon.transform.SetParent(transform);
        weapon.SetupSlotSizeMods(this.weaponSize);
    }

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
        rarityType = (WeaponRarity)Mathf.Min(pos, rarityRanges.Length - 1);
        SetupFromExisting();
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

    /*
    public bool TryLink(Ship ship, Arsenal.WeaponPosition position)
    {
        return ship.GetArsenal().TrySetWeapon(this, position);
    }
    */

    public AWeapon GetWeapon()
    {
        return weapon;
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
