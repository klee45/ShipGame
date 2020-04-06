using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Entity
{
    [SerializeField]
    protected CombatStats combatStats;

    protected override void Awake()
    {
        base.Awake();
        combatStats = GetComponentInChildren<CombatStats>();
        pilot = GetComponentInChildren<Pilot>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public CombatStats GetCombatStats() { return combatStats; }
}
