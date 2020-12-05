using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTemplate : ShipEffectTemplate
{
    [SerializeField]
    private SizeModNumber damage;
    [SerializeField]
    private float duration;

    private Ship owner;

    public override void Initialize()
    {
        base.Initialize();
        owner = GetComponentInParent<Ship>();
    }

    protected override ShipEffect CreateEffect(GameObject obj)
    {
        Burn burn = obj.AddComponent<Burn>();
        burn.Setup(damage.ToInt(), duration, owner);
        return burn;
    }
}
