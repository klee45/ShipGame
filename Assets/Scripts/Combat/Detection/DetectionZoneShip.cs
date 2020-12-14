using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZoneShip : DetectionZone<ShipCollider, Ship>
{
    protected override void DoInvoke(ShipCollider obj)
    {
        OnDetection?.Invoke(obj.GetShip());
    }

    protected override int InitializeLayer()
    {
        return Layers.DETECTION_SHIP;
    }
}
