using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddBarrier : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int val = 50;
    [SerializeField]
    private int limit = 100;

    public void Setup(int val, int limit)
    {
        this.val = val;
        this.limit = limit;
    }
    
    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add<HitAddBarrier>(this, () =>
            new EffectDictProjectile.OnHitEffectCase<HitAddBarrier>(true,
            new EffectDict.EffectList<ProjectileEffect.IOnHitEffect, HitAddBarrier>()));
    }

    public override string GetName()
    {
        return "Barrier";
    }

    public override EffectTag[] GetTags()
    {
        return TagHelper.empty;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        //Debug.Log(collision);
        Physics2D.IgnoreCollision(collidee, collision);
        Helper(collision.GetComponent<Ship>(), val, limit);
    }

    public static void Helper(Ship ship, int val, int limit)
    {
        ship.GetCombatStats().AddBarrier(val, limit);
    }
}