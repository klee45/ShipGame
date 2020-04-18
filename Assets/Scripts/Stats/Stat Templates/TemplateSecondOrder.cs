using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSecondOrder : StatGroupTemplate
{
    [SerializeField]
    private float initialValue;
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
        group.Setup(initialValue, acceleration, deceleration, max, min, dampening);
        return group;
    }

    public override float GetValue(float duration)
    {
        float maxSpeedTime = (max - initialValue) / acceleration;
        float halfAccelerating = initialValue * maxSpeedTime + acceleration * maxSpeedTime * maxSpeedTime / 2f;
        if (maxSpeedTime < duration)
        {
            float halfMaxSpeed = max * (duration - maxSpeedTime);
            return halfAccelerating + halfMaxSpeed;
        }
        return halfAccelerating;
    }
}
