using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Entity
{
    [SerializeField]
    protected CombatStats combatStats;
    [SerializeField]
    protected Pilot pilot;

    protected override void Awake()
    {
        base.Awake();
        combatStats = GetComponentInChildren<CombatStats>();
    }

    protected override void Update()
    {
        pilot?.MakeDecisions(this);
        base.Update();
    }

    public MovementStats GetMovementStats() { return movementStats; }
    public CombatStats GetCombatStats() { return combatStats; }
}
