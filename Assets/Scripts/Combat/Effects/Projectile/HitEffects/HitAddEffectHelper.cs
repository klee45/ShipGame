using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitAddEffectHelper<T, U> :
    ProjectileEffect,
    ProjectileEffect.IOnHitEffect
    where T : EffectTemplate<U>
    where U : Effect
{
    [SerializeField]
    protected T template;
    [SerializeField]
    private EffectTag[] tags;

    public void Setup(T template, EffectTag[] tags)
    {
        this.template = template;
        if (tags.Length == 0)
        {
            this.tags = TagHelper.empty;
        }
    }

    public override EffectTag[] GetTags()
    {
        return tags;
    }

    public abstract void OnHit(Collider2D collision, Collider2D collidee);
}
