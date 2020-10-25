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

    public List<GameObject> GetObjectsToTransfer()
    {
        return new List<GameObject>
        {
            playerTeam.gameObject
        };
    }
}
