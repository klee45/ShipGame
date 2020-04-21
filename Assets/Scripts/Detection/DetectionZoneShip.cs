using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZoneShip : DetectionZone<Ship>
{
    protected override int InitializeLayer()
    {
        return Layers.DETECTION_SHIP;
    }
}
