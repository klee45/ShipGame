using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCreateProjectileTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private ProjectileTemplate template;
    [SerializeField]
    private bool makeNeutral = false;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var hcp = obj.AddComponent<HitCreateProjectile>();
        hcp.Setup(template, makeNeutral);
        return hcp;
    }
}
