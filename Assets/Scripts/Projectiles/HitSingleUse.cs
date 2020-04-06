using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSingleUse : ProjectileOnHit
{
    [SerializeField]
    private int damage;

    public override void OnHit(Collider2D collision)
    {
        DoDamage(collision, damage);
        DestroySelf();
    }
}
