using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequiresShipSize
{
    void SetupShipSizeMods(Size shipSize);
}

public interface IRequiresSlotSize
{
    void SetupSlotSizeMods(Size slotSize);

}
