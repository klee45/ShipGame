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

    public override float GetValue(float duration)
    {
        return 0;
    }
}
public class StatGroupZeroOrder : StatGroup
{
    public override float GetValue()
    {
        return 0;
    }

    public override void Tick(float scale)
    {
    }
}
