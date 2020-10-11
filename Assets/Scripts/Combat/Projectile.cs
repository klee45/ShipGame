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

    private Ship owner;

    private EffectDictProjectile projectileEffects;

    protected override void Awake()
    {
        base.Awake();
        projectileEffects = gameObject.AddComponent<EffectDictProjectile>();
        foreach (ProjectileEffect effect in GetComponentsInChildren<ProjectileEffect>())
        {
            effect.AddTo(projectileEffects);
        }
    }

    protected void FixedUpdate()
    {
        DoFixedTickEffects(projectileEffects);
    }

    public void Setup(float range, float duration, EffectTag[] immuneTags)
    {
        this.range = range;
        this.duration = duration;
        this.projectileEffects.SetImmuneTags(immuneTags);
        SetupLifespan();
        SetupColor();
    }

    public void AddImmunityTag(EffectTag tag)
    {
        this.projectileEffects.AddImmuneTag(tag);
    }

    public override EffectDict GetGeneralEffectDict()
    {
        return projectileEffects;
    }

    public void SetOwner(Ship owner)
    {
        this.owner = owner;
    }

    public Ship GetOwner()
    {
        return owner;
    }

    private void SetupLifespan()
    {
        if (duration > 0)
        {
            AddProjectilEffect<FixedLifespan>().Setup(duration);
        }
    }

    protected override void Move(float rotation, float velocity)
    {
        float time = TimeController.DeltaTime(timeScale);
        transform.Rotate(new Vector3(0, 0, -rotation * time));
        transform.position += transform.up * velocity * time;
    }

    protected override void Translate(Vector2 translation)
    {
        transform.position += translation.ToVector3();
    }

    public override T AddEntityEffect<T>(EffectSetup<T> setup)
    {
        T e = projectileEffects.gameObject.AddComponent<T>();
        setup(e);
        e.AddTo(GetEffectsDict());
        return e;
    }

    public T AddProjectilEffect<T>() where T : ProjectileEffect
    {
        T p = projectileEffects.gameObject.AddComponent<T>();
        p.AddTo(GetEffectsDict());
        return p;
    }

    public EffectDictProjectile GetEffectsDict()
    {
        return projectileEffects;
    }

    protected override void ApplyEffects()
    {
        DoTickEffects(projectileEffects);
        DoMovementEffects(projectileEffects);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoTriggerEnter2D(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DoTriggerStay2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DoTriggerExit2D(collision);
    }
    */

    public void DoTriggerEnter2D(Collider2D collision, Collider2D collidee)
    {
        foreach (EffectDictProjectile.IOnHitEffectCase e in projectileEffects.onHits.GetAll())
        {
            e.OnHit(collision, collidee);
        }
        DoTriggerStay2D(collision);
    }

    public void DoTriggerStay2D(Collider2D collision)
    {
        foreach (EffectDictProjectile.IOnHitStayEffectCase e in projectileEffects.onStays.GetAll())
        {
            e.OnHitStay(collision);
        }
    }

    public void DoTriggerExit2D(Collider2D collision)
    {
        foreach (EffectDictProjectile.IOnExitEffectCase e in projectileEffects.onExits.GetAll())
        {
            e.OnExit(collision);
        }
    }
}
