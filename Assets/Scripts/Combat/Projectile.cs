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

    protected override void Awake()
    {
        base.Awake();
        projectileEffects = gameObject.AddComponent<EffectDictProjectile>();
        foreach (ProjectileEffect effect in GetComponentsInChildren<ProjectileEffect>())
        {
            effect.AddTo(projectileEffects);
        }
    }

    public void Setup(float range, float duration, Tag[] immuneTags)
    {
        this.range = range;
        this.duration = duration;
        this.projectileEffects.SetImmuneTags(immuneTags);
    }

    protected override void Start()
    {
        base.Start();
        if (duration > 0)
            Destroy(gameObject, duration);
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


    public override T AddEntityEffect<T>()
    {
        T e = projectileEffects.gameObject.AddComponent<T>();
        e.AddTo(GetEffectsDict());
        return e;
    }

    public EffectDictProjectile GetEffectsDict()
    {
        return projectileEffects;
    }

    protected override void ApplyEffects()
    {
        DoTickEffects(projectileEffects);
        DoMovementEffects(projectileEffects);
        DoGeneralEffects(projectileEffects);
    }

    private static void SortOrder(ProjectileEffect[] lst)
    {
        lst.OrderBy(p => p.GetPriority());
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
        foreach (var e in projectileEffects.onHits.GetAll())
        {
            e.OnHit(collision, collidee);
        }
    }

    public void DoTriggerStay2D(Collider2D collision)
    {
        foreach (var e in projectileEffects.onStays.GetAll())
        {
            e.OnHitStay(collision);
        }
    }

    public void DoTriggerExit2D(Collider2D collision)
    {
        foreach (var e in projectileEffects.onExits.GetAll())
        {
            e.OnExit(collision);
        }
    }
}
