using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDeed : Deed<AWeapon, Ship>
{
    [SerializeField]
    private string weaponName;
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private AWeapon weaponPrefab;
    [SerializeField]
    private int rarity;
    private WeaponRarity rarityType;

    private static int rarityDivisor = 100;

    private void Awake()
    {
        float value = rarity / rarityDivisor;
        int rarityCounts = typeof(WeaponRarity).GetCount() - 1;
        rarityType = (WeaponRarity)Mathf.FloorToInt(Mathf.Min(value, rarityCounts));
    }

    public override AWeapon Create(Ship ship)
    {
        AWeapon weapon = Instantiate(weaponPrefab);
        ship.GetArsenal().TrySetWeapon(weapon);
        return weapon;
    }

    public int GetRarity()
    {
        return rarity;
    }

    public WeaponRarity GetRarityType()
    {
        return rarityType;
    }

    public enum WeaponRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}
