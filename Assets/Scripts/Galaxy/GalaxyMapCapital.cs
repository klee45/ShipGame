using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxyMapCapital : GalaxyMapSector
{
    protected override string SetupName(string sectorID)
    {
        return "Capital " + sectorID;
    }

    private static Color ColorImageUnhighlighted = new Color(0.7f, 0.7f, 0.7f);

    protected override Color GetImageUnhighlighted()
    {
        return ColorImageUnhighlighted;
    }
}
