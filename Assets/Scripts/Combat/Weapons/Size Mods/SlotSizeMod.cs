using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSizeMod : SizeModNumber
{
    [SerializeField] private float smallMod = Constants.SizeMod.SMALL;
    [SerializeField] private float mediumMod = Constants.SizeMod.MEDIUM;
    [SerializeField] private float largeMod = Constants.SizeMod.LARGE;
    [SerializeField] private float hugeMod = Constants.SizeMod.HUGE;

    public override void SetupShip(Size shipSize)
    {
    }

    public override void SetupSlot(Size slotSize)
    {
        switch (slotSize)
        {
            case Size.Small:
                value = smallMod;
                break;
            case Size.Medium:
                value = mediumMod;
                break;
            case Size.Large:
                value = largeMod;
                break;
            case Size.Huge:
                value = hugeMod;
                break;
            default:
                Debug.LogError("Slot size mod recieved an invalid input size : " + slotSize.ToString());
                break;
        }
    }
}
