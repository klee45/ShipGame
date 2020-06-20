using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrder : StatGroup
{
    [SerializeField]
    private float multiplier;
    private FloatStat stat;
    private float currentValue;

    private void Start()
    {
        this.stat = new FloatStat(multiplier);
    }

    public void Setup(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public override float GetValue()
    {
        return currentValue;
    }

    public override void Tick(float scale, float deltaTime)
    {
        currentValue = stat.GetValue() * scale;
    }
}