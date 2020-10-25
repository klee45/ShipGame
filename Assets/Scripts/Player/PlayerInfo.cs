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
    private List<WeaponDeed> weaponDeeds;

    protected override void Awake()
    {
        base.Awake();
        weaponDeeds = new List<WeaponDeed>();
    }

    public void AddWeaponDeed(WeaponDeed deed)
    {
        Debug.Log("Added weapon deed!");
        this.weaponDeeds.Add(deed);
        deed.transform.SetParent(transform);
    }

    public bool RemoveWeaponDeed(WeaponDeed deed)
    {
        return this.weaponDeeds.Remove(deed);
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
