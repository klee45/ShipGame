using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSizeMod : SizeModNumber
{
    [SerializeField]
    private float mod = 1;

    public override void SetupShip(Size shipSize)
    {
    }

    public override void SetupSlot(Size slotSize)
    {
        value = mod;
    }
}
