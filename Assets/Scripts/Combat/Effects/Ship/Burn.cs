using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : ShipEffect, EntityEffect.ITickEffect, EffectDict.IEffectAdds<EntityEffect.ITickEffect>
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
        dict.tickEffects.Add(this);
    }

    public override string GetName()
    {
        return string.Format("Burn ({0})", damage);
    }

    private static Tag[] tags = new Tag[] { Tag.DAMAGE };
    public override Tag[] GetTags()
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
