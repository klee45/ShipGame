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
    }

    public void Setup(float range, float duration, ATimeScale timeScale)
    {
        this.range = range;
        this.duration = duration;
        this.timeScale = timeScale;
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

    public EffectDictProjectile GetEffectsDict()
    {
        return projectileEffects;
    }

    protected override void ApplyEffects()
    {
        DoTickEffects(projectileEffects);
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
