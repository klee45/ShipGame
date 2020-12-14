using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileEffectTemplate : EffectTemplate<ProjectileEffect>
{
}

public abstract class ProjectileEffect : Effect
{
    public abstract void AddTo(EffectDictProjectile dict);

    protected Ship GetShip(Collider2D collision)
    {
        /*
        Debug.LogWarning(collision.name);
        Debug.LogWarning(collision);
        Debug.LogWarning(collision.GetComponent<ShipCollider>());
        Debug.LogWarning(collision.GetComponent<ShipCollider>().GetShip());
        */
        return collision.GetComponent<ShipCollider>().GetShip();
    }

    protected Projectile GetProjectile(Collider2D collision)
    {
        return collision.GetComponent<ProjectileCollider>().GetProjectile();
    }

    protected Entity GetEntity(Collider2D collision)
    {
        return collision.GetComponent<EntityCollider>().GetEntity();
    }

    public interface IOnHitEffect : IEffect
    {
        void OnHit(Collider2D collision, Collider2D collidee);
    }

    public interface IOnHitStayEffect : IEffect
    {
        void OnHitStay(Collider2D collision);
    }

    public interface IOnExitEffect : IEffect
    {
        void OnExit(Collider2D collision);
    }

    protected void DestroySelf()
    {
        Destroy(GetComponent<Projectile>().gameObject);
    }

    protected void DoDamage(Collider2D collision, int damage, Ship source)
    {
        GetShip(collision).GetCombatStats().TakeDamage(damage, source);
    }
}
