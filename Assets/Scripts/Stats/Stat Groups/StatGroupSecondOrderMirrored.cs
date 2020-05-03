using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSecondOrderMirrored : StatGroupTemplate
{
    [SerializeField]
    private float initial;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float max;
    [SerializeField]
    private float dampening;

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupSecondOrderMirrored>();
        group.Setup(initial, acceleration, max, dampening);
        return group;
    }

    public override float GetValue(float duration)
    {
        return max * duration * acceleration * duration * duration / 2.0f;
    }
}

public class StatGroupSecondOrderMirrored : StatGroup
{
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

    public override float GetValue()
    {
        return kernel.GetValue();
    }

    public override void Tick(float scale)
    {
        kernel.Tick(scale);
    }
}