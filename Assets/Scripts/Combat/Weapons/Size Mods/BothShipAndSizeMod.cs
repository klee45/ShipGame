using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothShipAndSizeMod : SizeMod
{
    [SerializeField] BothModType modType = BothModType.multiplication;

    [Header("Ship size, slot size")]
    [SerializeField] private Vector2 smallMod = new Vector2(Constants.SizeMod.SMALL, Constants.SizeMod.SMALL);
    [SerializeField] private Vector2 mediumMod = new Vector2(Constants.SizeMod.MEDIUM, Constants.SizeMod.MEDIUM);
    [SerializeField] private Vector2 largeMod = new Vector2(Constants.SizeMod.LARGE, Constants.SizeMod.LARGE);
    [SerializeField] private Vector2 hugeMod = new Vector2(Constants.SizeMod.HUGE, Constants.SizeMod.HUGE);

    public override void SetupShip(Size shipSize)
    {
        float ship = GetFromSize(shipSize, 0);
        switch (modType)
        {
            case BothModType.addition:
                value += ship;
                break;
            case BothModType.multiplication:
                value *= ship;
                break;
            default:
                Debug.LogError("Both size modifier had an invalid type");
                break;
        }

    }

    public override void SetupSlot(Size slotSize)
    {
        value = GetFromSize(slotSize, 1);
    }

    private float GetFromSize(Size size, int index)
    {
        float val;
        switch (size)
        {
            case Size.Small:
                val = smallMod[index];
                break;
            case Size.Medium:
                val = mediumMod[index];
                break;
            case Size.Large:
                val = largeMod[index];
                break;
            case Size.Huge:
                val = hugeMod[index];
                break;
            default:
                Debug.LogError("Both ship and slot size mod recieved an invalid input size : " + size.ToString() + " at index " + index);
                val = 1;
                break;
        }
        return val;
    }

    private enum BothModType
    {
        addition,
        multiplication
    }
}
