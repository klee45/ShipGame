using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestructTemplate : ProjectileEffectTemplate
{
    private void Awake()
    {
        priority = -1000;
    }

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        HitSelfDestruct hitSelfDestruct = obj.AddComponent<HitSelfDestruct>();
        return hitSelfDestruct;
    }
}

