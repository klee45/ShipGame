using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmor :
    ShipEffect,
    EntityEffect.IGeneralEffect,
    EntityEffect.ITickEffect,
    Math.IStackableBonus
{
    [SerializeField]
    private int bonus;
    [SerializeField]
    private int limit;
    [SerializeField]
    private float duration;

    private Timer timer;

    public void Setup(int bonus, int max, float duration)
    {
        this.bonus = bonus;
        this.limit = max;
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
        dict.generalEffects.Add(this, () => new ModifyArmorGeneralEffectCase(true, dict.GetEntity(), new EffectDict.EffectList<EntityEffect.IGeneralEffect, ModifyArmor>()));
        dict.tickEffects.Add(this, () => new EffectDict.TickEffectCase<ModifyArmor>(false, new EffectDict.EffectList<EntityEffect.ITickEffect, ModifyArmor>()));
    }

    private class ModifyArmorGeneralEffectCase : EffectDict.AGeneralEffectCase<ModifyArmor>
    {
        private float mod = 0;

        public ModifyArmorGeneralEffectCase(bool isVisible, Entity affectedEntity, EffectDict.IEffectList<EntityEffect.IGeneralEffect, ModifyArmor> effectsList) : base(isVisible, affectedEntity, effectsList)
        {
        }

        public override string GetName()
        {
            return string.Format("Modify armor {0}%", (mod - 1) * 100);
        }

        public override void Apply(Entity entity)
        {
            if (entity is Ship s)
            {
                s.GetCombatStats().GetArmor().GetMult().Mult(Math.GetStackableBonusMod(effectsList.GetAll()));
            }
        }

        public override void Cleanup(Entity entity)
        {
            if (entity is Ship s)
            {
                s.GetCombatStats().GetArmor().GetMult().MultUndo(Math.GetStackableBonusModInverse(effectsList.GetAll()));
            }
        }
    }

    public void Tick(float timeScale)
    {
        timer.Tick(TimeController.DeltaTime(timeScale));
    }

    /*
    public void Apply(Entity e)
    {
        Debug.Log("Multiply by " + (1 + GetPercent()));
        e.GetComponentInChildren<CombatStats>().GetArmorMult().Mult(1 + GetPercent());
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
    */

    public override string GetName()
    {
        return "Modify Armor";
    }

    private static readonly EffectTag[] tags = new EffectTag[] { EffectTag.SHRED, EffectTag.SHRED_HULL };
    public override EffectTag[] GetTags()
    {
        return tags;
    }

    public int GetBonus()
    {
        return bonus;
    }

    public int GetLimit()
    {
        return limit;
    }
}
