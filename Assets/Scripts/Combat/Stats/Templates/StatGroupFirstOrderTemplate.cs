using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrderTemplate : StatGroupTemplate
{
    [SerializeField]
    private float multiplier;

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupFirstOrder>();
        group.Setup(multiplier);
        return group;
    }

    public override float GetDuration(float range)
    {
        return range / multiplier;
    }

    public override float GetRange(float duration)
    {
        return multiplier * duration;
    }
}
