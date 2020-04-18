using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrder : StatGroup
{
    [SerializeField]
    private float multiplier;
    private FloatStat stat;

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
        return stat.GetValue();
    }

    public override void Tick(float scale)
    {
    }
}