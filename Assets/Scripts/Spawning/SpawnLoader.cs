using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoader : Singleton<SpawnLoader>
{
    private static int numTeams = typeof(Team).GetCount();

    [SerializeField]
    private APilot pilot;

    [SerializeField]
    private bool[] teamsLoaded;
        
    private Dictionary<Team, Ship[][]> shipPrefabs;
    private List<WeaponDeed>[] deeds;
    private List<int>[] weaponWeights;

    protected override void Awake()
    {
        base.Awake();
        shipPrefabs = new Dictionary<Team, Ship[][]>();
        teamsLoaded = new bool[numTeams];
        ResetTeamsLoaded();
    }

    public void UnloadAllShips()
    {
        for (int teamNum = 0; teamNum < numTeams; teamNum++)
        {
            UnloadTeam((Team)teamNum);
        }
    }
    
    private void ResetTeamsLoaded()
    {
        for (int i = 0; i < teamsLoaded.Length; i++)
        {
            teamsLoaded[i] = false;
        }
    }

    public bool GetShipPrefabs(Team team, out Ship[][] prefabs)
    {
        try
        {
            prefabs = shipPrefabs[team];
            return true;
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning("Ships for team " + team + " not loaded yet\n" + e);
            prefabs = null;
            return false;
        }
    }

    public void UnloadWeapons()
    {
        foreach (List<WeaponDeed> deedList in deeds)
        {
            foreach (WeaponDeed deed in deedList)
            {
                Resources.UnloadAsset(deed);
            }
        }
        deeds = null;
    }

    public APilot GetAIPIlot()
    {
        return pilot;
    }

    public bool LoadTeams(HashSet<Team> teamsToLoad)
    {
        int successfulLoads = 0;
        for (int teamNum = 0; teamNum < teamsLoaded.Length; teamNum++)
        {
            Team team = (Team)teamNum;
            if (teamsLoaded[teamNum])
            {
                if (teamsToLoad.Contains(team))
                {
                    // Already loaded! No need to do anything
                    successfulLoads++;
                }
                else
                {
                    // Team is currently loaded but requested not to be
                    UnloadTeam(team);
                }
            }
            else
            {
                if (teamsToLoad.Contains(team))
                {
                    // Team is not loaded and there is a request
                    LoadTeam(team);
                    successfulLoads++;
                }
                else
                {
                    // Not loaded with no request
                }
            }
        }
        return successfulLoads == teamsToLoad.Count;
    }

    private void UnloadTeam(Team team)
    {
        int teamPos = (int)team;
        foreach (Ship[] ships in shipPrefabs[team])
        {
            foreach (Ship ship in ships)
            {
                Resources.UnloadAsset(ship);
            }
        }
        shipPrefabs[team] = null;
        teamsLoaded[teamPos] = false;
    }

    private void LoadTeam(Team team)
    {
        if (GetShipFolderFromTeam(team, out string folder))
        {
            shipPrefabs[team] = Loader.LoadFromFolder<Ship>("Ships/" + folder, "Small", "Medium", "Large");
            deeds = LoadDeeds();
        }
        teamsLoaded[(int)team] = true;
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
        int numSizes = typeof(Size).GetCount();
        List<WeaponDeed>[] deeds = new List<WeaponDeed>[numSizes];
        weaponWeights = new List<int>[numSizes];
        for (int i = 0; i < numSizes; i++)
        {
            deeds[i] = new List<WeaponDeed>();
            weaponWeights[i] = new List<int>();
        }

        WeaponDeed[] weaponDeeds = Resources.LoadAll<WeaponDeed>("Weapons");
        foreach (WeaponDeed deed in weaponDeeds)
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

}
