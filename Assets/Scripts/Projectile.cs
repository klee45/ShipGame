using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : Entity
{
    private ProjectileOnHit[] onHit;
    private ProjectileOnTick[] onTick;

    protected override void Awake()
    {
        base.Awake();
        OnValidate();
    }

    private void OnValidate()
    {
        onHit = GetComponents<ProjectileOnHit>();
        onTick = GetComponents<ProjectileOnTick>();

        onHit.OrderBy(p => p.GetPriority());
        onTick.OrderBy(p => p.GetPriority());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Trigger enter");
        foreach (var e in onHit)
        {
            e.OnHit(collision);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        foreach (var e in onTick)
        {
            e.OnTick();
        }
    }
}
