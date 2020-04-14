﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrder : StatGroup
{
    [SerializeField]
    private float acceleration, deceleration, max, min, dampening;

    private StatGroupSecondOrderKernel kernel;

    void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        kernel = new StatGroupSecondOrderKernel(acceleration, deceleration, max, min, dampening);
    }

    public override float GetValue()
    {
        return kernel.GetValue();
    }

    public override float GetValue(float duration)
    {
        return kernel.GetValue(duration);
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

    public StatGroupSecondOrderKernel(float acceleration, float deceleration, float max, float min, float dampening)
    {
        this.maxStat = new FloatStat(max);
        this.accelerationStat = new FloatStat(acceleration);
        this.minStat = new FloatStat(min);
        this.decelerationStat = new FloatStat(deceleration);
        this.dampening = dampening;
        currentValue = 0;
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

    public float GetValue(float duration)
    {
        return GetValue() * duration + accelerationStat.GetValue() * duration * duration / 2.0f;
    }

    /*
    public FloatStat GetAcceleration() { return acceleration; }
    public FloatStat GetDeceleration() { return deceleration; }
    public FloatStat GetMax() { return max; }
    public FloatStat GetMin() { return min; }
    */
}