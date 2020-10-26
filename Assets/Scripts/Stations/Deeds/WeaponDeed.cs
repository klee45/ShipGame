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
    private int energy = 0;
    [SerializeField]
    private float cooldownTime;

    [SerializeField]
    private Size weaponSize = Size.Medium;
    [SerializeField]
    private CombatType combatType = CombatType.Offense;
    [SerializeField]
    private int rarity = 100;

    [SerializeField]
    private Arsenal.WeaponPosition preferedPosition = Arsenal.WeaponPosition.Center;

    [SerializeField]
    private AWeapon weaponPrefab;
    private WeaponRarity rarityType;

    [SerializeField]
    private string damageString;
    [SerializeField]
    [TextArea(15, 20)]
    private string description = "";

    private static int[] rarityRanges = new int[] { 10, 25, 45, 70, 100 };

    private void Awake()
    {
        int pos = 0;
        foreach(int rarityCheck in rarityRanges)
        {
            if (rarity <= rarityCheck)
            {
                break;
            }
            pos++;
        }

        rarityType = (WeaponRarity)pos;
    }

    public override AWeapon Create(Ship ship)
    {
        AWeapon weapon = Instantiate(weaponPrefab);
        ship.GetArsenal().TrySetWeapon(weapon);
        weapon.Setup(
            icon, energy, cooldownTime,
            weaponSize, combatType, rarity,
            preferedPosition);
        return weapon;
    }

    public bool IsSame(WeaponDeed deed)
    {
        return
            deed.weaponName == weaponName &&
            deed.combatType == combatType &&
            deed.cooldownTime == cooldownTime &&
            deed.energy == energy &&
            deed.icon == icon &&
            deed.rarity == rarity &&
            deed.rarityType == rarityType &&
            deed.weaponSize == weaponSize;
    }

    public string GetName() { return weaponName; }
    public Sprite GetIcon() { return icon; }
    public int GetEnergyCost() { return energy; }
    public float GetCooldown() { return cooldownTime; }
    public Size GetSize() { return weaponSize; }
    public CombatType GetCombatType() { return combatType; }
    public int GetRarity() { return rarity; }
    public string GetDamageString() { return damageString; }
    public string GetDescription() { return description; }

    public WeaponRarity GetRarityType()
    {
        return rarityType;
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
