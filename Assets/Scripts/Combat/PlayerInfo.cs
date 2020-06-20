using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Singleton<PlayerInfo>
{
    [SerializeField]
    private TeamManager playerTeam;

    public List<GameObject> GetObjectsToTransfer()
    {
        return new List<GameObject>
        {
            playerTeam.gameObject
        };
    }
}
