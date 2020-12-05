using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddShipEffectTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private ShipEffectTemplate template;
    [SerializeField]
    private EffectTag[] tags;

    public override void Initialize()
    {
        base.Initialize();
        template.Initialize();
    }

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var s = obj.AddComponent<HitAddShipEffect>();
        s.Setup(template, tags);
        return s;
    }
}
