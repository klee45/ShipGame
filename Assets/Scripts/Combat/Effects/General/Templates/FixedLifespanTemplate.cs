using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespanTemplate : GeneralEffectTemplate
{
    [SerializeField]
    private float duration;

    protected override GeneralEffect CreateEffect(GameObject obj)
    {
        FixedLifespan fl = obj.AddComponent<FixedLifespan>();
        fl.Setup(duration);
        return fl;
    }
}
