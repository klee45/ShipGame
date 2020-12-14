using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : EntityCollider
{
    private Projectile parent;

    public void Setup(Projectile parent, int layer)
    {
        this.parent = parent;
        SetLayer(layer);
    }

    public Projectile GetProjectile()
    {
        return parent;
    }

    public override Entity GetEntity()
    {
        return parent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D collidee = GetComponent<Collider2D>();
        parent.DoTriggerEnter2D(collision, collidee);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        parent.DoTriggerStay2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parent.DoTriggerExit2D(collision);
    }
}
