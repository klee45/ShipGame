using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrder : StatGroup
{
    [SerializeField]
    private float initialValue = 0;
    [SerializeField]
    private float acceleration = 1;
    [SerializeField]
    private float deceleration = -1;
    [SerializeField]
    private float max = 1;
    [SerializeField]
    private float min = -1;
    [SerializeField]
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

    public override void Tick(float scale, float deltaTime)
    {
        kernel.Tick(scale, deltaTime);
    }

    public override float GetValue()
    {
        return kernel.GetValue();
    }

    public override void MultMod(float inc, float dec)
    {
        kernel.MultMod(inc, dec);
    }

    public override void MultModUndo(float inc, float dec)
    {
        kernel.MultModUndo(inc, dec);
    }
}

public class StatGroupSecondOrderKernel
{
    private static float INPUT_LIMIT = 0.1f;

    public ResettingFloat accelerationStat, decelerationStat, maxStat, minStat;

    private float dampening;
    private float currentValue;

    public StatGroupSecondOrderKernel(float initial, float acceleration, float deceleration, float max, float min, float dampening)
    {
        this.maxStat = new ResettingFloat(max);
        this.accelerationStat = new ResettingFloat(acceleration);
        this.minStat = new ResettingFloat(min);
        this.decelerationStat = new ResettingFloat(deceleration);
        this.dampening = dampening;
        currentValue = initial;
    }

    public void Tick(float scale, float deltaTime)
    {
        // Forwards
        if (scale > INPUT_LIMIT)
        {
            this.currentValue += accelerationStat.GetValue() * scale * deltaTime;
            this.currentValue = Mathf.Min(this.currentValue, maxStat.GetValue());
        }
        // Backwards
        else if (scale < -INPUT_LIMIT)
        {
            this.currentValue += decelerationStat.GetValue() * -scale * deltaTime;
            this.currentValue = Mathf.Max(this.currentValue, minStat.GetValue());
        }
        else
        {
            float mod = (1 - Mathf.Abs(scale)) * dampening * deltaTime;
            if (this.currentValue >= mod)
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

    public void MultMod(float inc, float dec)
    {
        accelerationStat.Mult(inc);
        decelerationStat.Mult(dec);
        maxStat.Mult(inc);
        minStat.Mult(dec);
    }

    public void MultModUndo(float inc, float dec)
    {
        accelerationStat.MultUndo(inc);
        decelerationStat.Mult(dec);
        maxStat.MultUndo(inc);
        minStat.MultUndo(dec);
    }
}