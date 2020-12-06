using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    private Projectile parent;

    public void Setup(Projectile parent)
    {
        this.parent = parent;
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
