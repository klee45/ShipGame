using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddEntityEffectTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private EntityEffectTemplate template;
    [SerializeField]
    private EffectTag[] tags;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var s = obj.AddComponent<HitAddEntityEffect>();
        s.Setup(template, tags);
        return s;
    }
}
