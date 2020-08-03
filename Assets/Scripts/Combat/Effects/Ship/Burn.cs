using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : ShipEffect, EntityEffect.ITickEffect
{
    [SerializeField]
    private int damage;

    int damageSoFar;
    private float dps;
    float leftover;
    private Ship ship;

    public void Setup(int damage, float duration)
    {
        this.damage = damage;
        this.dps = damage / duration;
        this.leftover = 0;
        this.damageSoFar = 0;
    }

    private void Start()
    {
        ship = GetComponentInParent<Ship>();
    }

    public override void AddTo(EffectDictShip dict)
    {
        dict.tickEffects.Add(this, () => new BurnEffectCase(new EffectDict.EffectList<EntityEffect.ITickEffect, Burn>()));
    }

    private class BurnEffectCase : EffectDictProjectile.TickEffectCase<Burn>
    {
        public BurnEffectCase(EffectDict.IEffectList<EntityEffect.ITickEffect, Burn> effectsList) : base(effectsList)
        {
        }

        public override void Update<V>(V effect)
        {
            base.Update(effect);
        }
    }

    public override string GetName()
    {
        return string.Format("Burn ({0})", damage);
    }

    private static EffectTag[] tags = new EffectTag[] { EffectTag.DAMAGE };
    public override EffectTag[] GetTags()
    {
        return tags;
    }

    public void Tick(float timeScale)
    {
        float currentDamage = leftover + dps * TimeController.DeltaTime(timeScale);
        int flatDamage = currentDamage.GetParts(out leftover);

        if (damageSoFar + flatDamage >= damage)
        {
            flatDamage = damage - damageSoFar;
            Destroy(this);
        }

        //Debug.Log(currentDamage);
        
        ship.GetCombatStats().TakeDamage(flatDamage);
        damageSoFar += flatDamage;
    }
}
