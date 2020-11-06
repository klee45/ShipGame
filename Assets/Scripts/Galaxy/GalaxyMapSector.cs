using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxyMapSector : GalaxyMapVertex
{
    protected override string SetupName(string sectorID)
    {
        return "Sector " + sectorID;
    }

    private static Color ColorTextUnhighlighted = Color.white;
    private static Color ColorImageUnhighlighted = Color.black;

    protected override Color GetTextUnhighlighted()
    {
        return ColorTextUnhighlighted;
    }

    protected override Color GetImageUnhighlighted()
    {
        return ColorImageUnhighlighted;
    }

    protected override Scene SetSector()
    {
        return SceneManager.GetSceneByName(sectorName);
    }
}
