using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrderMirrored : StatGroup
{
    [SerializeField]
    private float initial, acceleration, max, dampening;

    private StatGroupSecondOrderKernel kernel;

    void Start()
    {
        kernel = new StatGroupSecondOrderKernel(initial, acceleration, -acceleration, max, -max, dampening);
    }

    public void Setup(float initial, float acceleration, float max, float dampening)
    {
        this.initial = initial;
        this.acceleration = acceleration;
        this.max = max;
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