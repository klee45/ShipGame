using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTemplate : ShipEffectTemplate
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float duration;

    protected override ShipEffect CreateEffect(GameObject obj)
    {
        Burn burn = obj.AddComponent<Burn>();
        burn.Setup(damage, duration);
        return burn;
    }
}
