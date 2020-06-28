﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileEffectTemplate : EffectTemplate<ProjectileEffect>
{
}

public abstract class ProjectileEffect : Effect
{
    public abstract void AddTo(EffectDictProjectile dict);

    public interface IOnHitEffect : IEffect
    {
        void OnHit(Collider2D collision, Collider2D collidee);
    }

    public interface IOnHitStayEffect : IEffect
    {
        void OnHitStay(Collider2D collision);
    }

    public interface IOnEXitEffect : IEffect
    {
        void OnExit(Collider2D collision);
    }

    public interface IGeneralProjectileEffet : IGeneralEffectBase<Projectile>
    {
        void Apply(Projectile e);
        void Cleanup(Projectile e);
    }

    protected void DestroySelf()
    {
        Destroy(GetComponent<Projectile>().gameObject);
    }

    protected void DoDamage(Collider2D collision, int damage)
    {
        collision.gameObject.GetComponentInChildren<CombatStats>()?.TakeDamage(damage);
    }
}
