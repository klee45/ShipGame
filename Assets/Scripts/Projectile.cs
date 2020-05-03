using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectileTemplate : EntityTemplate<Projectile>
{
    [SerializeField]
    private EffectTemplate[] effects;
    [SerializeField]
    private float delay = 0;

    public EffectTemplate[] GetEffects()
    {
        return effects;
    }

    public float GetDelay()
    {
        return delay;
    }

    public override Projectile Create(GameObject obj)
    {
        Projectile projectile = obj.AddComponent<Projectile>();
        GameObject effectsObj = new GameObject();
        foreach (EffectTemplate effect in effects)
        {
            Effect e = effect.Create(effectsObj);
            e.AddTo(projectile.GetEffectsDict());
        }
        projectile.SetParent(obj);
        return projectile;
    }
}

public class Projectile : Entity
{
    private EffectDictProjectile projectileEffects;

    public void Setup(StatGroup velocityTemplate, StatGroup rotationTemplate, int damage, float lifespan)
    {
        MovementStats movementStats = GetComponentInChildren<MovementStats>();
        movementStats.Setup(velocityTemplate, rotationTemplate);
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, lifespan);
    }

    public override EffectDict GetEffectsDict()
    {
        return projectileEffects;
    }

    protected override void ApplyEffects()
    {
        projectileEffects.Tick();
        DoGenericEffects(projectileEffects);
        DoMovementEffects(projectileEffects);
    }

    public void SetLifespan(float lifespan)
    {
        this.lifespan = lifespan;
    }

    private static void SortOrder(ProjectileMod[] lst)
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
