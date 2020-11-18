using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrderScaled : AStatGroupFirstOrder
{
    [SerializeField]
    private SizeMod multiplier;

    protected override float GetMultiplier()
    {
        return multiplier.ToFloat();
    }
}
