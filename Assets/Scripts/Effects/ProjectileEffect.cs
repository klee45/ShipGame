using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileEffectTemplate : EffectTemplate<ProjectileEffect>
{
}

public abstract class ProjectileEffect : Effect
{
    public void AddTo(EffectDictProjectile dict)
    {
        AddToHelper(dict);
        OnDestroyEvent += () => RemoveFromHelper(dict);
    }

    protected abstract void AddToHelper(EffectDictProjectile dict);
    protected abstract void RemoveFromHelper(EffectDictProjectile dict);

    public interface IOnHitEffect : IEffect
    {
        void OnHit(Collider2D collision);
    }

    public interface IOnHitStayEffect : IEffect
    {
        void OnHitStay(Collider2D collision);
    }

    protected void DestroySelf()
    {
        Destroy(GetComponent<Projectile>().gameObject);
    }

    protected void DoDamage(Collider2D collision, int damage)
    {
        collision.gameObject.GetComponentInChildren<CombatStats>().TakeDamage(damage);
    }
}
