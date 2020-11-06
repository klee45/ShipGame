using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxyMapSubsector : GalaxyMapVertex
{
    protected override string SetupName(string sectorID)
    {
        return "Subsector " + sectorID;
    }

    private static Color ColorTextUnhighlighted = Color.white;
    private static Color ColorImageUnhighlighted = new Color(0.5f, 0.5f, 0.5f);

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
