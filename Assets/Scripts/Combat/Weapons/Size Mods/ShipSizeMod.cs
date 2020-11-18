using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSizeMod : SizeMod
{
    [SerializeField] private float smallMod = Constants.SizeMod.SMALL;
    [SerializeField] private float mediumMod = Constants.SizeMod.MEDIUM;
    [SerializeField] private float largeMod = Constants.SizeMod.LARGE;
    [SerializeField] private float hugeMod = Constants.SizeMod.HUGE;

    public override void SetupShip(Size shipSize)
    {
        switch (shipSize)
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
                Debug.LogError("Ship size mod recieved an invalid input size : " + shipSize.ToString());
                value = 0;
                break;
        }
    }

    public override void SetupSlot(Size slotSize)
    {
    }
}
