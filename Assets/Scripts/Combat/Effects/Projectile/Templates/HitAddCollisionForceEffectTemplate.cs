using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddCollisionForceEffectTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private float strength;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var s = obj.AddComponent<HitAddCollisionForceEffect>();
        s.Setup(duration, strength);
        return s;
    }
}
