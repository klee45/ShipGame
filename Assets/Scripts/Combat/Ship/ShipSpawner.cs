using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField]
    private APilot pilot;
    /*
    [SerializeField]
    private DetectionShip detection;
    */

    [SerializeField]
    private List<int> sizeWeights;
    [SerializeField]
    private List<int> teamWeights;

    private Ship[][] empire;
    private List<AWeapon>[] weapons;
    private List<int>[] weaponWeights;

    private void Awake()
    {
        sizeWeights.StackList();
        teamWeights.StackList();

        empire = Loader.LoadFromFolder<Ship>("Ships/Empire", "Small", "Medium", "Large");
        weapons = LoadWeapons();

        for (int i = 0; i < 100; i++)
        {
            CreateShip(
                CivilizationType.Empire,
                UnityEngine.Random.Range(-10f, 10f),
                UnityEngine.Random.Range(-10f, 10f),
                true);
        }
    }

    private List<AWeapon>[] LoadWeapons()
    {
        int numSizes = Enum.GetValues(typeof(Size)).Length;
        List<AWeapon>[] weapons = new List<AWeapon>[numSizes];
        weaponWeights = new List<int>[numSizes];
        for (int i = 0; i < numSizes; i++)
        {
            weapons[i] = new List<AWeapon>();
            weaponWeights[i] = new List<int>();
        }
        AWeapon[] allWeapons = Resources.LoadAll<AWeapon>("Weapons");
        foreach(AWeapon weapon in allWeapons)
        {
            int pos = (int)weapon.GetSize();
            weapons[pos].Add(weapon);
            weaponWeights[pos].Add(weapon.GetRarity());
        }
        foreach (List<int> lst in weaponWeights)
        {
            lst.StackList();
        }
        return weapons;
    }

    public AWeapon GetRandomWeaponPrefab(Size size)
    {
        int pos = (int)size;
        List<AWeapon> sized = weapons[pos];
        List<int> weights = weaponWeights[pos];
        AWeapon weapon = sized[Math.WeightedRandom(weights)];
        return weapon;
    }

    private Size GetRandomSize()
    {
        return (Size)Math.WeightedRandom(sizeWeights);
    }

    private Team GetRandomTeam()
    {
        return (Team)Math.WeightedRandom(teamWeights);
    }

    public void CreateShip(CivilizationType civilization, float x, float y, bool fill=false)
    {
        Size size = GetRandomSize();
        Team team = GetRandomTeam();
        Ship[][] allShips = GetShips(civilization);
        
        Ship prefab = allShips[(int)size].GetRandomElement();
        Ship ship = Instantiate(prefab);
        ship.SetParent(gameObject);
        ship.transform.position = new Vector2(x, y);
        ship.SetTeam(team);
        APilot shipPilot = Instantiate(pilot);
        ship.SetPilot(shipPilot);
        shipPilot.transform.SetParent(ship.transform);

        Arsenal arsenal = ship.GetArsenal();
        int[] slots = arsenal.GetSlots();
        for (int weaponSizeIndex = 0; weaponSizeIndex < slots.Length; weaponSizeIndex++)
        {
            if (weapons[weaponSizeIndex].Count <= 0)
            {
                Debug.Log("Weapons of size " + (Size)weaponSizeIndex + " don't exist yet");
            }
            else
            {
                int count = slots[weaponSizeIndex];
                if (!fill && count != 0)
                {
                    count = UnityEngine.Random.Range(1, count);
                }
                for (int i = 0; i < count; i++)
                {
                    AWeapon weapon = Instantiate<AWeapon>(GetRandomWeaponPrefab((Size)weaponSizeIndex));
                    bool result = arsenal.TrySetWeapon(weapon);
                    if (!result)
                    {
                        Destroy(weapon.gameObject);
                    }
                }
            }
        }
    }

    private Ship[][] GetShips(CivilizationType civilization)
    {
        switch(civilization)
        {
            case CivilizationType.Empire:
                return empire;
        }
        return null;
    }
}
