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
    [SerializeField]
    private float percentFillWeapons;

    [SerializeField]
    private int minRarity = -1;
    [SerializeField]
    private int maxRarity = -1;

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

    private List<Ship> ships;

    private bool active = false;

    private void Awake()
    {
        ships = new List<Ship>();

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
        try
        {
            Ship prefab = shipPrefabs[size].GetRandomElement();
            Ship ship = CreateShip(prefab);
            ships.Add(ship);
            ship.OnEntityDestroy += (i) => RemoveShip(ship);
            if (ships.Count >= maxSpawns)
            {
                active = false;
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError("Couldn't create ship from prefab with size " + size + "\nShip prefabs is of size " + shipPrefabs.Length);
        }
    }

    public void RemoveShip(Ship ship)
    {
        if (ships.Count < maxSpawns)
        {
            active = true;
        }
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

    public Ship CreateShip(Ship prefab)
    {
        Ship ship = Instantiate(prefab);
        ship.SetParent(gameObject);
        ship.transform.position = transform.position;
        ship.SetTeam(team);
        APilot shipPilot = Instantiate(pilot);
        ship.SetPilot(shipPilot);
        shipPilot.transform.SetParent(ship.transform);

        Arsenal arsenal = ship.GetArsenal();
        int[] slots = arsenal.GetSlots();
        int totalWeapons = 0;
        for (int slot = 0; slot < slots.Length; slot++)
        {
            Size size = (Size)slot;
            int slotCount = slots[slot];
            if (slotCount > 0)
            {
                if (DropTable.instance.GetDrops(size, minRarity, maxRarity, out List<int> weights, out List<DropTable.DropInfo> drops))
                {
                    int numWeapons = Mathf.CeilToInt(slotCount / percentFillWeapons);

                    for (int weaponNum = 0; weaponNum < Mathf.Max(1, numWeapons); weaponNum++)
                    {
                        DropTable.DropInfo info = drops[Math.WeightedRandom(weights)];
                        WeaponDeed deed = Instantiate(info.deed);
                        deed.Setup(info.info);
                        ship.GetArsenal().TrySetWeapon(deed, deed.GetWeapon().GetPreferedPosition(), totalWeapons++);
                        if (totalWeapons > 8)
                        {
                            return ship;
                        }
                    }
                }
            }
        }
        return ship;
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
