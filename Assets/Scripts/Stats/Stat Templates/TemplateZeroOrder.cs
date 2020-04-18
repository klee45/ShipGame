using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateZeroOrder : StatGroupTemplate
{
    public override StatGroup CreateGroup(GameObject attachee)
    {
        var group = attachee.AddComponent<StatGroupZeroOrder>();
        return group;
    }

    public override float GetValue(float duration)
    {
        return 0;
    }
}
