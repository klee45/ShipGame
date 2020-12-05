using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnceTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private SizeModNumber damage;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        Ship source = obj.GetComponent<Projectile>().GetOwner();
        HitOnce ho = obj.AddComponent<HitOnce>();
        ho.Setup(damage.ToInt(), source);
        return ho;
    }
}
