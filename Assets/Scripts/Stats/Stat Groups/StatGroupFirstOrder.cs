using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrder : StatGroup
{
    [SerializeField]
    private float value;

    private FloatStat stat;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        this.stat = new FloatStat(value);
    }

    public override float GetValue()
    {
        return stat.GetValue();
    }

    public override void Tick(float scale)
    {
    }

    public override float GetValue(float duration)
    {
        return GetValue() * duration;
    }
}