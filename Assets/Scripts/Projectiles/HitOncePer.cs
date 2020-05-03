﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOncePer : ProjectileMod, ProjectileEffect.IOnHitEffect
{
    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    public void OnHit(Collider2D collision)
    {
        Entity hitEntity = collision.GetComponent<Entity>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), hitEntity.GetComponent<Collider2D>());
        DoDamage(collision, GetDamage());
    }

    public void Tick() { }
}
