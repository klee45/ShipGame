using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrderMirrored : StatGroup
{
    [SerializeField]
    private float acceleration, max, dampening;

    private StatGroupSecondOrderKernel kernel;

    private void Awake()
    {
        kernel = new StatGroupSecondOrderKernel(acceleration, -acceleration, max, -max, dampening);
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