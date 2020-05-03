using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateFirstOrder : StatGroupTemplate
{
    [SerializeField]
    private float multiplier;

    public override StatGroup Create(GameObject obj)
    {
        var group = obj.AddComponent<StatGroupFirstOrder>();
        group.Setup(multiplier);
        return group;
    }

    public override float GetValue(float duration)
    {
        return multiplier * duration;
    }
}

public class StatGroupFirstOrder : StatGroup
{
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