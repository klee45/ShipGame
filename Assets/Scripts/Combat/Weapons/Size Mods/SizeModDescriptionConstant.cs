using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeModDescriptionConstant : SizeModDescription
{
    [SerializeField]
    [TextArea(4, 6)]
    private string str;

    public override string GetDescription()
    {
        return str;
    }

    public override void SetupShip(Size shipSize)
    {
    }

    public override void SetupSlot(Size slotSize)
    {
    }
}
