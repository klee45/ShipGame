using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    private Projectile parent;

    private void Awake()
    {
        parent = GetComponentInParent<Projectile>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.DoTriggerEnter2D(collision);
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
