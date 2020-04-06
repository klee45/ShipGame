using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSingle : ProjectileOnHit
{
    [SerializeField]
    private int damage;

    private List<Entity> alreadyHit;

    private void Awake()
    {
        alreadyHit = new List<Entity>();
    }

    public override void OnHit(Collider2D collision)
    {
        Entity hitEntity = collision.GetComponent<Entity>();
        // Not hit already
        if (!alreadyHit.Contains(hitEntity))
        {
            DoDamage(collision, damage);
            alreadyHit.Add(hitEntity);
        }
    }
}
