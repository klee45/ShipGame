using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrderMirrored : StatGroup
{
    [SerializeField]
    private float initialValue, acceleration, max, dampening;

    private StatGroupSecondOrderKernel kernel;

    void Start()
    {
        kernel = new StatGroupSecondOrderKernel(initialValue, acceleration, -acceleration, max, -max, dampening);
    }

    public void Setup(float acceleration, float max, float dampening)
    {
        this.acceleration = acceleration;
        this.max = max;
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