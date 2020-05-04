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
    private float percent = 1.0f;
    [SerializeField]
    private bool isRelative = true;

    protected override GeneralEffect CreateEffect(GameObject obj)
    {
        Force f = obj.AddComponent<Force>();
        f.Setup(force, duration, percent, isRelative);
        return f;
    }

    public override float GetRangeMod()
    {
        return force.y * (duration + (duration * duration / 2.0f));
    }
}

