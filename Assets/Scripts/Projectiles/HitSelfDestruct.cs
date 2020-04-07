using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfDestruct : ProjectileOnHit
{
    private void Awake()
    {
        priority = -1000;
    }

    public override void OnHit(Collider2D collision)
    {
        DestroySelf();
    }
}
