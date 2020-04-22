using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateFirstOrder : StatGroupTemplate
{
    [SerializeField]
    private float multiplier;

    public override StatGroup CreateGroup(GameObject attachee)
    {
        var group = attachee.AddComponent<StatGroupFirstOrder>();
        group.Setup(multiplier);
        return group;
    }

    public override float GetValue(float duration)
    {
        return multiplier * duration;
    }
}
