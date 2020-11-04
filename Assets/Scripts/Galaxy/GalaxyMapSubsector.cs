using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyMapSubsector : GalaxyMapVertex
{
    protected override string SetupName(string sectorID)
    {
        return "Subsector " + sectorID;
    }
}
