using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeModEffect : EntityEffect, EntityEffect.IGeneralEffect, EffectDict.IEffectAdds<EntityEffect.IGeneralEffect>
{
    [SerializeField]
    private float scale;

    public void Setup(float scale)
    {
        this.scale = scale;
    }

    public override void AddTo(EffectDict dict)
    {
        dict.generalEffects.Add(this);
    }

    public override string GetName()
    {
        return string.Format("Time mod effect {0:#}x", scale);
    }

    public static readonly Tag[] tags = new Tag[] { Tag.TIME };
    public override Tag[] GetTags()
    {
        return tags;
    }

    public void Apply(Entity e)
    {
        e.GetTimeScale().Mult(scale);
        //Debug.Log("Scale: " + scale);
        //Debug.Log(e.GetTimeScale().GetValue());
    }

    public void Cleanup(Entity e)
    {
        e.GetTimeScale().MultUndo(1f / scale);
    }
}
