using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionShip : Detection<Ship>
{
    public bool GetHealthiest(out Ship ship)
    {
        return GetHelper(out ship, GetHealthHelper, (a, b) => a > b);
    }

    public bool GetLeastHealthy(out Ship ship)
    {
        return GetHelper(out ship, GetHealthHelper, (a, b) => a < b);
    }

    protected override DetectionZone<Ship> InitializeZone(GameObject zone)
    {
        return zone.AddComponent<DetectionZoneShip>();
    }

    private void GetHealthHelper(Ship ship, ref Ship result, Comp comparer)
    {
        float val = ship.GetCombatStats().GetOverallPercent();
        if (comparer(val, result.GetCombatStats().GetOverallPercent()))
        {
            result = ship;
        }
    }


}
