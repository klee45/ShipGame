using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrder : StatGroup
{
    [SerializeField]
    private float baseValue;
    private ResettingFloat stat;
    private float currentValue;

    private void Awake()
    {
        this.stat = new ResettingFloat(baseValue);
    }

    public void Setup(float multiplier)
    {
        this.baseValue = multiplier;
        Awake();
    }

    public override void ForcePercent(float percent)
    {
        currentValue = stat.GetValue() * percent;
    }

    public override float GetValue()
    {
        return currentValue;
    }

    public override void Tick(float scale, float deltaTime)
    {
        currentValue = stat.GetValue() * scale;
    }

    public ResettingFloat GetStat()
    {
        return stat;
    }

    public override void MultMod(float inc, float dec)
    {
        stat.Mult(inc);
    }

    public override void MultModUndo(float inc, float dec)
    {
        stat.MultUndo(inc);
    }
}