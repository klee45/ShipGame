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

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupSecondOrder>();
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

public class StatGroupSecondOrder : StatGroup
{
    private float initialValue = 0;
    private float acceleration = 1;
    private float deceleration = 1;
    private float max = 1;
    private float min = -1;
    private float dampening = 1;

    private StatGroupSecondOrderKernel kernel;

    void Start()
    {
        kernel = new StatGroupSecondOrderKernel(initialValue, acceleration, deceleration, max, min, dampening);
    }

    public void Setup(float initialValue, float acceleration, float deceleration, float max, float min, float dampening)
    {
        this.initialValue = initialValue;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
        this.max = max;
        this.min = min;
        this.dampening = dampening;
    }

    public override float GetValue()
    {
        return kernel.GetValue();
    }

    public override void Tick(float scale)
    {
        kernel.Tick(scale);
    }
}

public class StatGroupSecondOrderKernel
{
    private static float INPUT_LIMIT = 0.25f;
    private FloatStat accelerationStat, decelerationStat, maxStat, minStat;
    private float dampening;
    private float currentValue;

    public StatGroupSecondOrderKernel(float initial, float acceleration, float deceleration, float max, float min, float dampening)
    {
        this.maxStat = new FloatStat(max);
        this.accelerationStat = new FloatStat(acceleration);
        this.minStat = new FloatStat(min);
        this.decelerationStat = new FloatStat(deceleration);
        this.dampening = dampening;
        currentValue = initial;
    }

    public void Tick(float scale)
    {
        // Forwards
        if (scale > INPUT_LIMIT)
        {
            this.currentValue += accelerationStat.GetValue() * scale * Time.deltaTime;
            this.currentValue = Mathf.Min(this.currentValue, maxStat.GetValue());
        }
        // Backwards
        else if (scale < -INPUT_LIMIT)
        {
            this.currentValue += decelerationStat.GetValue() * -scale * Time.deltaTime;
            this.currentValue = Mathf.Max(this.currentValue, minStat.GetValue());
        }
        else
        {
            float mod = (1 - Mathf.Abs(scale)) * dampening * Time.deltaTime;
            if (this.currentValue > mod)
            {
                this.currentValue -= mod;
            }
            else if (this.currentValue < -mod)
            {
                this.currentValue += mod;
            }
            else
            {
                this.currentValue = 0;
            }
        }
    }

    public float GetValue()
    {
        return currentValue;
    }

    /*
    public FloatStat GetAcceleration() { return acceleration; }
    public FloatStat GetDeceleration() { return deceleration; }
    public FloatStat GetMax() { return max; }
    public FloatStat GetMin() { return min; }
    */
}