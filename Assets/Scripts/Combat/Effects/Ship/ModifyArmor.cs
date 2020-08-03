using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmor :
    ShipEffect,
    EntityEffect.IGeneralEffect,
    EntityEffect.ITickEffect
{
    [SerializeField]
    private int bonus;
    [SerializeField]
    private int max;
    [SerializeField]
    private float duration;

    private Timer timer;

    public void Setup(int bonus, int max, float duration)
    {
        this.bonus = bonus;
        this.max = max;
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(duration);
        timer.OnComplete += () =>
        {
            Destroy(this);
            Destroy(timer);
        };
        OnDestroyEvent += (e) => Destroy(timer);
    }

    public override void AddTo(EffectDictShip dict)
    {
        dict.generalEffects.Add(this, () => new ModifyArmorGeneralEffectCase(dict.GetEntity(), new EffectDict.EffectList<EntityEffect.IGeneralEffect, ModifyArmor>()));
        dict.tickEffects.Add(this, () => new EffectDict.TickEffectCase<ModifyArmor>(new EffectDict.EffectList<EntityEffect.ITickEffect, ModifyArmor>()));
    }

    private class ModifyArmorGeneralEffectCase : EffectDict.AGeneralEffectCase<ModifyArmor>
    {
        public ModifyArmorGeneralEffectCase(Entity affectedEntity, EffectDict.IEffectList<EntityEffect.IGeneralEffect, ModifyArmor> effectsList) : base(affectedEntity, effectsList)
        {
        }

        public override void Apply(Entity entity)
        {
            int mod = 0;
            foreach (ModifyArmor effect in effectsList.GetAll())
            {
                mod += effect.bonus;
            }
        }

        public override void Cleanup(Entity entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public void Apply(Entity e)
    {
        Debug.Log("Multiply by " + (1 + GetPercent()));
        e.GetComponentInChildren<CombatStats>().GetArmorMult().Mult(1 + GetPercent());
    }

    public void Tick(float timeScale)
    {
        timer.Tick(TimeController.DeltaTime(timeScale));
    }

    public void Cleanup(Entity e)
    {
        Debug.Log("Cleanup by " + (1 / (1 + GetPercent())));
        e.GetComponentInChildren<CombatStats>().GetArmorMult().MultUndo(1 / (1 + GetPercent()));
    }

    private float GetPercent()
    {
        return bonus / 100f;
    }

    private void SetDuration(float duration)
    {
        this.duration = duration;
        this.timer.SetMaxTime(duration);
    }

    public EntityEffect.ITickEffect UpdateEffect(EntityEffect.ITickEffect effect, out bool didReplace)
    {
        if (effect is ModifyArmor e)
        {
            if (this.duration > e.duration)
            {
                e.SetDuration(this.duration);
            }
            e.timer.SetTime(0);
        }
        didReplace = false;
        return effect;
    }

    public EntityEffect.IGeneralEffect UpdateEffect(EntityEffect.IGeneralEffect effect, out bool didReplace)
    {
        Debug.Log("Update4 effect");
        if (effect is ModifyArmor e)
        {
            e.bonus = Math.MaxBonus(e.bonus, this.bonus, e.max, this.max);
            /*
            int sum = e.bonus + this.bonus;
            if (this.max > e.max)
            {
                e.bonus = Mathf.Min(this.max, sum);
            }
            else
            {
                if (e.bonus < this.max)
                {
                    e.bonus = Mathf.Min(this.max, sum);
                }
            }
            */
        }
        didReplace = false;
        return effect;
    }

    public override string GetName()
    {
        return string.Format("Modify armor {0}%", bonus);
    }

    private static readonly EffectTag[] tags = new EffectTag[] { EffectTag.SHRED, EffectTag.SHRED_HULL };
    public override EffectTag[] GetTags()
    {
        return tags;
    }
}
