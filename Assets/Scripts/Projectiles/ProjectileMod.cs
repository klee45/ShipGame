using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileMod : ProjectileEffect
{
    protected int GetDamage()
    {
        return GetComponent<Projectile>().GetDamage();
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

public abstract class ProjectileOnHit : ProjectileMod
{
    public abstract void OnHit(Collider2D collision);
}

public abstract class ProjectileOnStay : ProjectileMod
{
    public abstract void OnHitStay(Collider2D collision);
}

public abstract class ProjectileOnTick : ProjectileMod
{
    public abstract void OnTick();
}
