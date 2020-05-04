using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrderTemplate : StatGroupTemplate
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

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupSecondOrder>();
        group.Setup(initialValue, acceleration, deceleration, max, min, dampening);
        return group;
    }

    public override float GetRange(float duration)
    {
        return GetRangeHelper(duration, initialValue, max, acceleration);
    }

    public override float GetDuration(float range)
    {
        return GetDurationHelper(range, initialValue, max, acceleration);
    }

    public static float GetRangeHelper(float duration, float initialValue, float max, float acceleration)
    {
        float maxSpeedTime = (max - initialValue) / acceleration;
        float maxSpeedRange = GetRangeBeforeMax(maxSpeedTime, initialValue, acceleration);
        if (maxSpeedTime > duration)
        {
            return maxSpeedRange + max * (duration - maxSpeedTime);
        }
        else
        {
            return GetRangeBeforeMax(duration, initialValue, acceleration);
        }
    }

    private static float GetRangeBeforeMax(float duration, float initialValue, float acceleration)
    {
        return initialValue * duration + acceleration * duration * duration / 2f;
    }

    public static float GetDurationHelper(float range, float initialValue, float max, float acceleration)
    {
        float maxSpeedTime = (max - initialValue) / acceleration;
        float maxSpeedRange = GetRangeBeforeMax(maxSpeedTime, initialValue, acceleration);
        if (range > maxSpeedRange)
        {
            return maxSpeedTime + (range - maxSpeedRange) * (1 / max);
        }
        else
        {
            return GetDurationBeforeMax(range, initialValue, acceleration);
        }
    }

    private static float GetDurationBeforeMax(float range, float initialValue, float acceleration)
    {
        float bsqr = initialValue * initialValue;
        float ac = 4 * acceleration * range;
        return (Mathf.Sqrt(bsqr + ac) - initialValue) / (2 * acceleration);
    }
}
