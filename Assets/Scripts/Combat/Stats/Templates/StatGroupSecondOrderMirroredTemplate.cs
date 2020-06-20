using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrderMirroredTemplate : StatGroupTemplate
{
    [SerializeField]
    private float initialValue;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float max;
    [SerializeField]
    private float dampening;

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupSecondOrderMirrored>();
        group.Setup(initialValue, acceleration, max, dampening);
        return group;
    }

    public override float GetDuration(float range)
    {
        return StatGroupSecondOrderTemplate.GetDurationHelper(range, initialValue, max, acceleration);
    }

    public override float GetRange(float duration)
    {
        return StatGroupSecondOrderTemplate.GetRangeHelper(duration, initialValue, max, acceleration);
    }
}
