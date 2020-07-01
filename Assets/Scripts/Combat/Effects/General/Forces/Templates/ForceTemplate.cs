using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTemplate : EntityEffectTemplate
{
    [SerializeField]
    private Vector2 force;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool isRelative = true;

    protected override EntityEffect CreateEffect(GameObject obj)
    {
        if (duration > 0)
        {
            Force f = obj.AddComponent<Force>();
            f.Setup(force, duration, isRelative);
            return f;
        }
        else
        {
            ForceEndless f = obj.AddComponent<ForceEndless>();
            f.Setup(force, isRelative);
            return f;
        }
    }

    public override float GetRangeMod()
    {
        return force.y * duration;
    }
}

