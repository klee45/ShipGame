using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SizeMod : MonoBehaviour
{
    public abstract void SetupSlot(Size slotSize);
    public abstract void SetupShip(Size shipSize);
}
