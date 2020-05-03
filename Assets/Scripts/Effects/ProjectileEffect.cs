using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileEffect : Effect
{
    public abstract void AddTo(EffectDictProjectile dict);

    public interface IOnHitEffect : IEffect
    {
        void OnHit(Collider2D collision);
    }

    public interface IOnHitStayEffect : IEffect
    {
        void OnHitStay(Collider2D collision);
    }
}
