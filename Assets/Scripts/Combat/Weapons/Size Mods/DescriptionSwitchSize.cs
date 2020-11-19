using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionSwitchSize : DescriptionSwitch
{
    [SerializeField]
    [TextArea(4, 6)]
    private string small;
    [SerializeField]
    [TextArea(4, 6)]
    private string medium;
    [SerializeField]
    [TextArea(4, 6)]
    private string large;
    [SerializeField]
    [TextArea(4, 6)]
    private string huge;

    private string str;

    public override string GetDescription()
    {
        return str;
    }

    public override void Setup(Size slotSize)
    {
        switch (slotSize)
        {
            case Size.Small:
                str = small;
                break;
            case Size.Medium:
                str = medium;
                break;
            case Size.Large:
                str = large;
                break;
            case Size.Huge:
                str = huge;
                break;
        }
        //Debug.Log("-------------- Description switch setup " + str);
    }
}
