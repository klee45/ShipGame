using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupZeroOrderTemplate : StatGroupTemplate
{
    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupZeroOrder>();
        return group;
    }

    public override float GetDuration(float range)
    {
        return -1;
    }

    public override float GetRange(float duration)
    {
        return 0;
    }
}