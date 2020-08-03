using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeModEffect :
    EntityEffect,
    EntityEffect.IGeneralEffect,
    Math.IStackableBonus
{ 
    [SerializeField]
    private int bonus;
    [SerializeField]
    private int limit;

    public void Setup(int bonus, int limit)
    {
        this.bonus = bonus;
        this.limit = limit;
    }

    public override void AddTo(EffectDict dict)
    {
        dict.generalEffects.Add(this, () => new TimeModeEffectCase(true, dict.GetEntity(), new EffectDict.EffectList<IGeneralEffect, TimeModEffect>()));
    }

    public override string GetName()
    {
        return string.Format("Time mod effect {0:0.##}x", 1 + bonus / 100f);
    }

    public static readonly EffectTag[] tags = new EffectTag[] { EffectTag.TIME };
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

    /*
    public void Apply(Entity e)
    {
        //Debug.Log("Timescale apply");
        e.GetTimeScale().Mult((1 + bonus / 100f));
        //Debug.Log("Scale: " + scale);
        //Debug.Log(e.GetTimeScale().GetValue());
    }

    public void Cleanup(Entity e)
    {
        e.GetTimeScale().MultUndo(1f / (1 + bonus / 100f));
    }
    */

    private class TimeModeEffectCase : EffectDict.AGeneralEffectCase<TimeModEffect>
    {
        public TimeModeEffectCase(bool isVisible, Entity affectedEntity, EffectDict.IEffectList<IGeneralEffect, TimeModEffect> effectsList) : base(isVisible, affectedEntity, effectsList)
        {
        }

        public override void Apply(Entity entity)
        {
            //Debug.Log("Time mod - apply");
            entity.GetTimeScale().Mult(Math.GetStackableBonusMod(effectsList.GetAll()));
        }

        public override void Cleanup(Entity entity)
        {
            //Debug.Log("Time mod - cleanup");
            entity.GetTimeScale().MultUndo(Math.GetStackableBonusModInverse(effectsList.GetAll()));
        }
    }

    /*
    public IGeneralEffect UpdateEffect(IGeneralEffect effect, out bool didReplace)
    {
        if (effect is TimeModEffect t)
        {
            if (this.bonus < 0)
            {
                t.bonus = Math.MinBonus(t.bonus, this.bonus, t.limit, this.limit);
            }
            else
            {
                t.bonus = Math.MaxBonus(t.bonus, this.bonus, t.limit, this.limit);
            }
        }
        didReplace = false;
        return effect;
    }
    */
}
