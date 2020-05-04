using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnceTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private int damage;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        HitOnce ho = obj.AddComponent<HitOnce>();
        ho.Setup(damage);
        return ho;
    }
}
