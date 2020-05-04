using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : Entity
{
    [SerializeField]
    private float range;
    [SerializeField]
    private float duration;

    private EffectDictProjectile projectileEffects;

    public void Awake()
    {
        projectileEffects = gameObject.AddComponent<EffectDictProjectile>();
    }

    public void Setup(float range, float duration)
    {
        this.range = range;
        this.duration = duration;
    }

    public void Start()
    {
        if (duration > 0)
            Destroy(gameObject, duration);
    }

    public EffectDictProjectile GetEffectsDict()
    {
        return projectileEffects;
    }

    protected override void ApplyEffects()
    {
        projectileEffects.Tick();
        DoGenericEffects(projectileEffects);
        DoMovementEffects(projectileEffects);
    }

    private static void SortOrder(ProjectileEffect[] lst)
    {
        lst.OrderBy(p => p.GetPriority());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var e in projectileEffects.onHits.GetAll())
        {
            e.OnHit(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (var e in projectileEffects.onStays.GetAll())
        {
            e.OnHitStay(collision);
        }
    }
}
