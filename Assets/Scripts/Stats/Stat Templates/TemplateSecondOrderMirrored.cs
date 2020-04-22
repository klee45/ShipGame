using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSecondOrderMirrored : StatGroupTemplate
{
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float max;
    [SerializeField]
    private float dampening;

    public override StatGroup CreateGroup(GameObject attachee)
    {
        var group = attachee.AddComponent<StatGroupSecondOrderMirrored>();
        group.Setup(acceleration, max, dampening);
        return group;
    }

    public override float GetValue(float duration)
    {
        return max * duration * acceleration * duration * duration / 2.0f;
    }
}
