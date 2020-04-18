using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSecondOrder : StatGroupTemplate
{
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float max;
    [SerializeField]
    private float min;
    [SerializeField]
    private float dampening;

    public override StatGroup CreateGroup(GameObject attachee)
    {
        var group = attachee.AddComponent<StatGroupSecondOrder>();
        group.Setup(acceleration, deceleration, max, min, dampening);
        return group;
    }

    public override float GetValue(float duration)
    {
        return max * duration * acceleration * duration * duration / 2.0f;
    }
}
