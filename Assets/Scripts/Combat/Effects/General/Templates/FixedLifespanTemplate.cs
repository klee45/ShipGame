using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespanTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private float duration;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        FixedLifespan fl = obj.AddComponent<FixedLifespan>();
        fl.Setup(duration);
        return fl;
    }
}
