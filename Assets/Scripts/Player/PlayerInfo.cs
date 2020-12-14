using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Singleton<PlayerInfo>
{
    [SerializeField]
    private TeamManager playerTeam;
    [SerializeField]
    private Ship ship;
    [SerializeField]
    private Bank bank;

    [SerializeField]
    private Inventory inventory;

    private GalaxyMapVertex currentLocation;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(playerTeam);
        Camera.main.GetComponent<FollowGameObject>().SetTarget(ship.gameObject);
    }

    private void Start()
    {
        InventoryInterface.instance.GetEquipmentUI().SetShip(ship);
        UIManager.instance.SetShip(ship);
    }

    public void SetLocation(GalaxyMapVertex vertex)
    {
        this.currentLocation = vertex;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public Bank GetBank()
    {
        return bank;
    }

    public TeamManager GetTeamToTransfer()
    {
        return playerTeam;
    }
}
