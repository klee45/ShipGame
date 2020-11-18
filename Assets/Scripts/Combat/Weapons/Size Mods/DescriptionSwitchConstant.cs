using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionSwitchConstant : DescriptionSwitch
{
    [SerializeField]
    [TextArea(4, 6)]
    private string str;

    public override string GetDescription()
    {
        return str;
    }

    public override void Setup(Size slotSize)
    {
    }
}
