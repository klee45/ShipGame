using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField]
    List<Weapon> weapons;
    [SerializeField]
    List<Ship> ships;

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

    private void Awake()
    {
        StackList(sizeWeights);
        StackList(teamWeights);

        empire = LoadShips("Ships/Empire");
        Debug.Log(empire.Length);
        Debug.Log(empire[0][0]);

        CreateShip(CivilizationType.Empire);
    }

    private Ship[][] LoadShips(string path)
    {
        Ship[] small = Resources.LoadAll<Ship>(path + "/small");
        Ship[] medium = Resources.LoadAll<Ship>(path + "/medium");
        Ship[] large = Resources.LoadAll<Ship>(path + "/large");
        Ship[] huge = Resources.LoadAll<Ship>(path + "/huge");
        return new Ship[][] { small, medium, large, huge };
    }

    private Size GetRandomSize()
    {
        return (Size)Math.WeightedRandom(sizeWeights);
    }

    private Team GetRandomTeam()
    {
        return (Team)Math.WeightedRandom(teamWeights);
    }

    public void CreateShip(CivilizationType civilization)
    {
        Size size = GetRandomSize();
        Team team = GetRandomTeam();
        Ship[][] allShips = GetShips(civilization);

        Ship prefab = allShips[(int)size].GetRandomElement();
        Ship ship = Instantiate(prefab);
        ship.SetParent(gameObject);
        ship.SetTeam(team);
        APilot shipPilot = Instantiate(pilot);
        ship.SetPilot(shipPilot);
        shipPilot.transform.SetParent(ship.transform);
    }

    private void StackList(List<int> lst)
    {
        for (int i = 1; i < lst.Count; i++)
        {
            lst[i] += lst[i - 1];
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
