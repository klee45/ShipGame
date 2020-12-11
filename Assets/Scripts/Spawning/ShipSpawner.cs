using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Place on a warp gate and it will spawn ships
 **/
public class ShipSpawner : MonoBehaviour
{
    [Header("Ship spawn information")]
    [SerializeField]
    private APilot pilot;
    [SerializeField]
    private Team team;
    [SerializeField]
    private List<int> sizeWeights;
    [SerializeField]
    private int maxWeaponSize;

    [Header("Spawn rates and max count")]
    [SerializeField]
    private float minSpawnDelay;
    [SerializeField]
    private float maxSpawnDelay;
    [SerializeField]
    private int maxSpawns;

    private Timer spawnTimer;

    private Ship[][] shipPrefabs;
    private List<WeaponDeed>[] deeds;
    private List<int>[] weaponWeights;

    private bool active = false;

    private void Awake()
    {
        if (GetShipFolderFromTeam(team, out string folder))
        {
            spawnTimer = gameObject.AddComponent<Timer>();
            SetTimer();
            spawnTimer.OnComplete += () => SetTimer();
            active = true;
            sizeWeights.StackList();

            shipPrefabs = Loader.LoadFromFolder<Ship>("Ships/" + folder, "Small", "Medium", "Large");
            deeds = LoadDeeds();
        }
    }

    private void Update()
    {
        if (active && spawnTimer.Tick(TimeController.StaticDeltaTime()))
        {
            GenerateShip();
        }
    }

    private void GenerateShip()
    {
        int size = Math.WeightedRandom(sizeWeights);
        Ship prefab = shipPrefabs[size].GetRandomElement();
        CreateShip(prefab);
    }

    private void SetTimer()
    {
        spawnTimer.SetMaxTime(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
        spawnTimer.SetTime(0);
    }

    private bool GetShipFolderFromTeam(Team team, out string folder)
    {
        switch (team)
        {
            case Team.Empire:
            case Team.Federation:
            case Team.UpperEmpire:
            case Team.Boss:
                folder = team.ToString();
                return true;
            default:
                Debug.LogWarning("Unsupported team for ship spawner (" + team.ToString() + ")");
                folder = "";
                return false;
        }
    }

    private List<WeaponDeed>[] LoadDeeds()
    {
        int numSizes = Enum.GetValues(typeof(Size)).Length;
        List<WeaponDeed>[] deeds = new List<WeaponDeed>[numSizes];
        weaponWeights = new List<int>[numSizes];
        for (int i = 0; i < numSizes; i++)
        {
            deeds[i] = new List<WeaponDeed>();
            weaponWeights[i] = new List<int>();
        }

        WeaponDeed[] weaponDeeds = Resources.LoadAll<WeaponDeed>("Weapons");
        foreach(WeaponDeed deed in weaponDeeds)
        {
            int pos = (int)deed.GetSize();
            deeds[pos].Add(deed);
            weaponWeights[pos].Add(1);
        }
        foreach (List<int> lst in weaponWeights)
        {
            lst.StackList();
        }
        return deeds;
    }

    public WeaponDeed GetRandomWeaponPrefab(Size size)
    {
        int pos = (int)size;
        List<WeaponDeed> sized = deeds[pos];
        List<int> weights = weaponWeights[pos];
        WeaponDeed deed = sized[Math.WeightedRandom(weights)];
        return deed;
    }

    private Size GetRandomSize()
    {
        return (Size)Math.WeightedRandom(sizeWeights);
    }

    private Team GetRandomTeam()
    {
        return (Team)Math.WeightedRandom(teamWeights);
    }

    public void CreateShip(Ship prefab, float x, float y, bool fill=false)
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
            if (deeds[weaponSizeIndex].Count <= 0)
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
                    WeaponDeed deed = Instantiate<WeaponDeed>(GetRandomWeaponPrefab((Size)weaponSizeIndex));
                    bool result = deed.Create(ship);
                    if (!result)
                    {
                        Destroy(deed.gameObject);
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
                return shipPrefabs;
        }
        return null;
    }
}
