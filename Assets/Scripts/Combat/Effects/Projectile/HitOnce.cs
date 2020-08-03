using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnce : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private int damage;

    public void Setup(int damage)
    {
        this.damage = damage;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        DoDamage(collision, damage);
        // Case does the destroying
        //DestroySelf();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new HitOnceEffectCase(new EffectDict.EffectList<IOnHitEffect, HitOnce>()));
    }

    private class HitOnceEffectCase : EffectDictProjectile.OnHitEffectCase<HitOnce>
    {
        public HitOnceEffectCase(EffectDict.IEffectList<IOnHitEffect, HitOnce> effectsList) : base(effectsList)
        {
        }

        public override void OnHit(Collider2D collision, Collider2D collidee)
        {
            base.OnHit(collision, collidee);
            Destroy(effectsList.GetFirst().GetComponent<Projectile>().gameObject);
        }
    }

    public override string GetName()
    {
        return "Hit once";
    }

    public static readonly EffectTag[] tags = new EffectTag[] { EffectTag.DAMAGE };
    public override EffectTag[] GetTags()
    {
        return tags;
    }
}
