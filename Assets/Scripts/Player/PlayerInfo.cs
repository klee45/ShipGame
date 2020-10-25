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

    protected override void Awake()
    {
        base.Awake();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public Bank GetBank()
    {
        return bank;
    }

    public List<GameObject> GetObjectsToTransfer()
    {
        return new List<GameObject>
        {
            playerTeam.gameObject
        };
    }
}
