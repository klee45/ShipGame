using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrderScaled : AStatGroupFirstOrder
{
    [SerializeField]
    private SizeModNumber multiplier;

    protected override float GetMultiplier()
    {
        return multiplier.ToFloat();
    }
}
