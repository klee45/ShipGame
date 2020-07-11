using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmor : ShipEffect, EntityEffect.IGeneralEffect, EntityEffect.ITickEffect, EffectDict.IEffectUpdates
{
    [SerializeField]
    private float mult;
    [SerializeField]
    private float duration;

    private Timer timer;

    public void Setup(float mult, float duration)
    {
        this.mult = mult;
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(duration);
        timer.OnComplete += () => Destroy(this);
    }

    public override void AddTo(EffectDictShip dict)
    {
        dict.generalEffects.AddUpdate(this);
        dict.tickEffects.AddUpdate(this);
    }

    public void Apply(Entity e)
    {
        e.GetComponentInChildren<CombatStats>().GetArmorMult().Mult(mult);
    }

    public void Tick(float timeScale)
    {
        timer.Tick(TimeController.DeltaTime(timeScale));
    }

    public void Cleanup(Entity e)
    {
        e.GetComponentInChildren<CombatStats>().GetArmorMult().MultUndo(1 / mult);
    }

    public IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
            
    }

    public override string GetName()
    {
        return string.Format("Modify armor {0:.#}%", mult);
    }

    private static readonly Tag[] tags = new Tag[] { Tag.SHRED, Tag.SHRED_HULL };
    public override Tag[] GetTags()
    {
        return tags;
    }
}
