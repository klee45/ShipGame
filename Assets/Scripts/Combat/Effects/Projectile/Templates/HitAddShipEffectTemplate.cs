using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddShipEffectTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private ShipEffectTemplate template;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var s = obj.AddComponent<HitAddShipEffect>();
        s.Setup(template);
        return s;
    }
}
