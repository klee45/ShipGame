﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupSecondOrderMirrored : StatGroup
{
    [SerializeField]
    private float acceleration, max, dampening;

    private StatGroupSecondOrderKernel kernel;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        kernel = new StatGroupSecondOrderKernel(acceleration, -acceleration, max, -max, dampening);
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