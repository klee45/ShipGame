using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPerSecond : ProjectileEffect,
    EntityEffect.IFixedTickEffect,
    ProjectileEffect.IOnHitStayEffect,
    ProjectileEffect.IOnHitEffect,
    EffectDict.IEffectAdds
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool destroyOnEnd = true;

    private int currentDamage;
    private int damageDone;
    private float leftover;
    private float dps;
    
    private void Start()
    {
        currentDamage = 0;
        damageDone = 0;
        leftover = 0;
        dps = damage / duration;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onStays.Add(this);
        dict.onHits.Add(this);
        dict.fixedTickEffects.Add(this);
    }

    public void Setup(int damage, float duration, bool destroyOnEnd)
    {
        this.damage = damage;
        this.duration = duration;
        this.destroyOnEnd = destroyOnEnd;
    }


    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        OnHitStay(collision);
    }

    public void OnHitStay(Collider2D collision)
    {
        if (currentDamage + damageDone > damage)
        {
            WakeUp(collision);
            DoDamage(collision, damage - damageDone);
        }
        else
        {
            WakeUp(collision);
            DoDamage(collision, currentDamage);
        }
        Debug.Log("Damage");
    }

    public void FixedTick(float timeScale)
    {
        Debug.Log(damageDone);
        if (damageDone > damage)
        {
            if (destroyOnEnd)
            {
                DestroySelf();
            }
        }
        else
        {
            damageDone += currentDamage;
            float damage = leftover + TimeController.FixedDeltaTime(timeScale) * dps;
            currentDamage = damage.GetParts(out leftover);
        }
        Debug.Log("Tick");  
    }

    private void WakeUp(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().WakeUp();
    }

    public override string GetName()
    {
        return "Hit on tick";
    }

    public static readonly Tag[] tags = new Tag[] { Tag.DAMAGE };
    public override Tag[] GetTags()
    {
        return tags;
    }
}
