using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSingle : ProjectileOnHit
{
    public override void OnHit(Collider2D collision)
    {
        DoDamage(collision, GetDamage());
    }
}
