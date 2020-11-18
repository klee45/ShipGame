using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGroupFirstOrderTemplate : AStatGroupFirstOrder
{
    [SerializeField]
    private float multiplier;

    protected override float GetMultiplier()
    {
        return multiplier;
    }
}
