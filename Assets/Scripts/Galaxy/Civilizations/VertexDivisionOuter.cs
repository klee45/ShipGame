using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexDivisionOuter : VertexDivision
{
    public VertexDivisionOuter(float empire, float federation, float avalon = 0) : base(empire, federation, avalon)
    {
    }

    protected override Team SetupWarpGate(WarpGate gate, GalaxyMapVertex source)
    {
        Vector2 gateSectorPos = gate.GetSector().GetSpacePosition();
        Vector2 sectorPos = source.GetSpacePosition();

        Vector2 direction = gateSectorPos - sectorPos;

        float angleToSectorFromCenter = sectorPos.ToAngle().ToDegree();
        float angleFromSectorToGate = direction.ToAngle().ToDegree();

        float diff = Mathf.Abs(angleToSectorFromCenter - angleFromSectorToGate);

        Debug.Log(source.name + " to " + gate.GetSector().name + " : " + diff + " from " + angleFromSectorToGate);

        ShipSpawner spawner = gate.gameObject.AddComponent<ShipSpawner>();
        Team team;

        float strength;
        
        if (diff <= 180)
        {
            team = Team.Empire;
            strength = empire * (1 - (diff / 180));
        }
        else
        {
            team = Team.Federation;
            strength = federation * (1 - ((diff - 180) / 180));
        }

        Debug.Log(strength + " : (" + empire + ", " + federation + ")");

        // Setup the spawner
        spawner.Setup(team, strength);
        return team;
    }
}
