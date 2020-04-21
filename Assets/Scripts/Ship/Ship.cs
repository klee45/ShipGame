using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Entity
{
    [SerializeField]
    protected CombatStats combatStats;

    private Arsenal arsenal;

    private bool markedForDelete = false;

    protected override void Awake()
    {
        base.Awake();
        combatStats = GetComponentInChildren<CombatStats>();
        pilot = GetComponentInChildren<Pilot>();
        arsenal = GetComponentInChildren<Arsenal>();
    }

    protected override void Start()
    {
        base.Start();
        combatStats.OnDeath += (d) =>
        {
            //Debug.Log(string.Format("Destroy {0}", gameObject.name));
            markedForDelete = true;
        };
    }

    private void FixedUpdate()
    {
        if(markedForDelete)
        {
            Destroy(gameObject);
        }
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
