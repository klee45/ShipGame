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
    [SerializeField]
    private bool doesFade = false;

    protected override EntityEffect CreateEffect(GameObject obj)
    {
        if (duration > 0)
        {
            Force f = obj.AddComponent<Force>();
            f.Setup(force, duration, isRelative, doesFade);
            return f;
        }
        else
        {
            ForceEndless f = obj.AddComponent<ForceEndless>();
            if (doesFade)
            {
                Debug.LogWarning("Endless force can't fade! " + gameObject);
            }
            f.Setup(force, isRelative);
            return f;
        }
    }

    public override float GetRangeMod()
    {
        if (doesFade)
        {
            return force.y * duration / 2;
        }
        else
        {
            return force.y * duration;
        }
    }
}

