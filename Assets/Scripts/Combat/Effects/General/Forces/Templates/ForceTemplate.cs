using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTemplate : GeneralEffectTemplate
{
    [SerializeField]
    private Vector2 force;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool isRelative = true;

    protected override EntityEffect CreateEffect(GameObject obj)
    {
        Force f = obj.AddComponent<Force>();
        f.Setup(force, duration, isRelative);
        return f;
    }

    public override float GetRangeMod()
    {
        return force.y * duration;
    }
}

