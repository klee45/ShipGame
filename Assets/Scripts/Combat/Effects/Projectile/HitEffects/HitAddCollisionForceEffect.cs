using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddCollisionForceEffect :
    ProjectileEffect,
    ProjectileEffect.IOnHitEffect,
    EffectDict.IEffectAdds<ProjectileEffect.IOnHitEffect>
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private float strength;

    public void Setup(float duration, float strength)
    {
        this.duration = duration;
        this.strength = strength;
    }

    public override string GetName()
    {
        return "Hit add collision force effect ";
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    public override Tag[] GetTags()
    {
        return TagHelper.empty;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Vector3 diff = (collision.transform.position - collidee.transform.position);
        Vector2 norm = diff.ToVector2().normalized;
        Force force = collision.GetComponent<Entity>().AddEntityEffect<Force>((e) => e.Setup(norm * strength, duration, false));
    }
}
