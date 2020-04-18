using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Entity
{
    [SerializeField]
    protected CombatStats combatStats;

    private Arsenal arsenal;

    protected override void Awake()
    {
        base.Awake();
        combatStats = GetComponentInChildren<CombatStats>();
        pilot = GetComponentInChildren<Pilot>();
        arsenal = GetComponentInChildren<Arsenal>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public CombatStats GetCombatStats() { return combatStats; }
    public Arsenal GetArsenal() { return arsenal; }
    public Weapon GetWeapon(int weaponPos)
    {
        return arsenal.GetWeapon(weaponPos);
    }
}
