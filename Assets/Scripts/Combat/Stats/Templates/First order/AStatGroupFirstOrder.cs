using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStatGroupFirstOrder : StatGroupTemplate
{
    protected abstract float GetMultiplier();

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupFirstOrder>();
        group.Setup(GetMultiplier());
        return group;
    }

    public override float GetDuration(float range)
    {
        return range / GetMultiplier();
    }

    public override float GetRange(float duration)
    {
        return GetMultiplier() * duration;
    }
}
