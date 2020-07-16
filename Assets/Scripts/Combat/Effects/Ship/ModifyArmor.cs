using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmor :
    ShipEffect,
    EntityEffect.IGeneralEffect,
    EntityEffect.ITickEffect,
    EffectDict.IEffectUpdates<EntityEffect.IGeneralEffect>,
    EffectDict.IEffectUpdates<EntityEffect.ITickEffect>
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
        dict.generalEffects.AddUpdate(this);
        dict.tickEffects.AddUpdate(this);
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
        }
        didReplace = false;
        return effect;
    }

    public override string GetName()
    {
        return string.Format("Modify armor {0}%", bonus);
    }

    private static readonly Tag[] tags = new Tag[] { Tag.SHRED, Tag.SHRED_HULL };
    public override Tag[] GetTags()
    {
        return tags;
    }
}
