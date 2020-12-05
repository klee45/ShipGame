using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOncePerTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private SizeModNumber damage;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        HitOncePer hop = obj.AddComponent<HitOncePer>();
        hop.Setup(damage.ToInt(), obj.GetComponent<Projectile>().GetOwner());
        return hop;
    }
}
