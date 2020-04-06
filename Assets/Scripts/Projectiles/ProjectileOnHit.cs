using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileOnHit : ProjectileEffect
{
    public abstract void OnHit(Collider2D collision);
}

public abstract class ProjectileEffect : MonoBehaviour
{
    [SerializeField]
    protected int priority = 0;

    public int GetPriority() { return priority; }
    
    protected void DestroySelf()
    {
        Destroy(GetComponentInParent<Projectile>().gameObject);
    }

    protected void DoDamage(Collider2D collision, int damage)
    {
        collision.gameObject.GetComponentInChildren<CombatStats>().TakeDamage(damage);
    }
}
