using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Place on a warp gate and it will spawn ships
 **/
public class ShipSpawner : MonoBehaviour
{
    [Header("Ship spawn information")]
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
    private List<Ship> ships;

    private bool active = false;

    public void Setup(Team team, float strength)
    {
        this.team = team;

        ships = new List<Ship>();
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.OnComplete += () => SetTimer();
        active = true;

        SetupStats(strength);
        SetTimer();
    }

    private void SetupStats(float strength)
    {
        // Small and medium ships are less likely the higher the strength
        // The inverse is the opposite for large and huge ships
        /**
        https://www.wolframalpha.com/input/?i=floor%2815+*+%281-x%29%29+%2B+2+%2B+0.3%2C+floor%284+*+%281-x%29%29+%2B+8+%2B+0.2%2C+floor%286*x%29+%2B+1+%2B+0.1%2C+floor%283.5*x%29+%2B+1+for+x+%3D+%7B0%2C+1%7D
        **/
        sizeWeights = new List<int>
        {
            Mathf.FloorToInt(15 * (1 - strength)) + 1,
            Mathf.FloorToInt(6 * (1 - strength)) + 3,
            Mathf.FloorToInt(8 * strength) + 1,
            Mathf.FloorToInt(5f * strength) + 1
        };
        sizeWeights.StackList();

        // Max weapon size is medium for the first 1/3 strength
        // then large for 2/3 and huge for 3/3
        /**
        https://www.wolframalpha.com/input/?i=plot+floor%283+*+%28x%29%29+%2B+2+for+x+%3D+%7B0%2C+1%7D
        **/
        maxWeaponSize = Mathf.FloorToInt(3 * strength) + 2;

        // Average spawn time range is [16, 8]
        float averageSpawnTime = 8 * (1 - strength) + 8;
        // Min spawn times range is [15, 7]
        minSpawnDelay = averageSpawnTime - strength - 1;
        // Max spawn times range is [20, 10]
        maxSpawnDelay = averageSpawnTime + (1 - strength) * 2 + 2;

        // spawns range from [3, 7] (per warpgate!)
        maxSpawns = 3 + Mathf.FloorToInt(strength * 4);

        // Percentage of weapons from [0.5, 1] (Full weapons at 0.8 strength)
        /**
        https://www.wolframalpha.com/input/?i=plot+min%281%2C+0.5+%2B+x+*+%285+%2F+8%29%29+for+x+%3D+%7B0%2C+1%7D
        **/
        percentFillWeapons = Mathf.Min(1f, 0.5f + strength * (5 / 8));
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

        if (SpawnLoader.instance.GetShipPrefabs(team, out Ship[][] shipPrefabs))
        {
            try
            {
                Ship prefab = shipPrefabs[size].GetRandomElement();
                Ship ship = CreateShip(prefab);
                ships.Add(ship);
                ship.transform.localEulerAngles = new Vector3(0, 0, 180);
                ship.OnEntityDestroy += (i) => RemoveShip(ship);
                ship.GetMovementStats().ForcePercentValues(0.5f, 0);
                if (ships.Count >= maxSpawns)
                {
                    active = false;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("Couldn't create ship from prefab with size " + size + "\nShip prefabs is of size " + shipPrefabs.Length + "\n" + e);
            }
        }
        else
        {
            Debug.LogWarning("Could not get loaded ships prefabs for team " + team);
        }
    }

    public void RemoveShip(Ship ship)
    {
        ships.Remove(ship);
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
    
    public Ship CreateShip(Ship prefab)
    {
        Ship ship = Instantiate(prefab);
        ship.SetParent(gameObject);
        ship.transform.position = transform.position;
        ship.SetTeam(team);
        APilot shipPilot = Instantiate(SpawnLoader.instance.GetAIPIlot());
        ship.SetPilot(shipPilot);
        shipPilot.transform.SetParent(ship.transform);
        AddWeapons(ship);

        /*
        // Needs rewrite to better fill slots
        for (int slot = 0; slot < slotCounts.Length; slot++)
        {
            Size size = (Size)slot;
            int slotCount = slotCounts[slot];
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
                        bool success = ship.GetArsenal().TrySetWeapon(deed, deed.GetWeapon().GetPreferedPosition(), totalWeapons++);
                        Debug.Log(success + " in trying to set " + deed.name + " with size " + deed.GetSize());
                        if (totalWeapons > 8)
                        {
                            return ship;
                        }
                    }
                }
            }
        }
        */
        return ship;
    }

    private void AddWeapons(Ship ship)
    {
        Arsenal arsenal = ship.GetArsenal();

        int[] slotCounts = arsenal.GetSlots();
        int numWeapons = Mathf.CeilToInt(slotCounts.Sum() * percentFillWeapons);
        List<int> availableSlotPositions = new List<int>();
        List<DropTable.DropInfo>[] allDrops = new List<DropTable.DropInfo>[slotCounts.Length];
        List<int>[] allWeights = new List<int>[slotCounts.Length];
        for (int i = 0; i < slotCounts.Length; i++)
        {
            if (slotCounts[i] > 0)
            {
                availableSlotPositions.Add(i);
                DropTable.instance.GetDrops((Size)i, minRarity, maxRarity, out List<int> weights, out List<DropTable.DropInfo> drops);
                allDrops[i] = drops;
                allWeights[i] = weights;
            }
        }

        for (int pos = 0; pos < numWeapons; pos++)
        {
            int chosenSlot;
            if (availableSlotPositions.Count > 1)
            {
                chosenSlot = availableSlotPositions.GetRandomElement();
            }
            else
            {
                chosenSlot = availableSlotPositions.First();
            }

            int chosenWeaponPos = allWeights[chosenSlot].WeightedRandom();
            DropTable.DropInfo info = allDrops[chosenSlot][chosenWeaponPos];
            WeaponDeed deed = Instantiate(info.deed);
            deed.Setup(info.info);
            bool success = ship.GetArsenal().TrySetWeapon(deed, deed.GetWeapon().GetPreferedPosition(), pos);

            string str = " in trying to set " + deed.name + " with size " + deed.GetSize() + " on " + ship.name;
            if (success)
            {
                Debug.Log(success + str);
            }
            else
            {
                Debug.LogWarning(success + str);
            }

            if (--slotCounts[chosenSlot] <= 0)
            {
                availableSlotPositions.Remove(chosenSlot);
            }
        }
    }

    /*
    private Ship[][] GetShips(CivilizationType civilization)
    {
        switch(civilization)
        {
            case CivilizationType.Empire:
                return shipPrefabs;
        }
        return null;
    }
    */
}
