using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSingle : ProjectileOnHit
{
    [SerializeField]
    private int damage;

    public override void OnHit(Collider2D collision)
    {
        collision.gameObject.GetComponentInChildren<CombatStats>().TakeDamage(damage);
        Death();
    }
}
