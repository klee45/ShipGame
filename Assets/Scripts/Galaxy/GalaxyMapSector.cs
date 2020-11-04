using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalaxyMapSector : GalaxyMapVertex
{
    protected override string SetupName(string sectorID)
    {
        return "Sector " + sectorID;
    }
}
