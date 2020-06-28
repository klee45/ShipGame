using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespanTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool doesFade = false;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        if (doesFade)
        {
            FixedLifespanFade fl = obj.AddComponent<FixedLifespanFade>();
            fl.Setup(duration);
            return fl;
        }
        else
        {
            FixedLifespan fl = obj.AddComponent<FixedLifespan>();
            fl.Setup(duration);
            return fl;
        }
    }
}
