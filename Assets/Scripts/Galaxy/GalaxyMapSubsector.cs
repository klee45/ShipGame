using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxyMapSubsector : GalaxyMapVertex
{
    private const string subsectorScene = "Subsector";

    [SerializeField]
    private SectorComponent[] components;

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

    protected override string SetSceneName()
    {
        return subsectorScene;
    }

    public override void SetupMap()
    {
        foreach(SectorComponent component in components)
        {
            component.Setup();
        }
    }

    protected override Vector3 SetSpacePosition()
    {
        float parentScale = transform.parent.localScale.x;
        Vector2 parentPos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
        return GetComponent<RectTransform>().anchoredPosition * parentScale + parentPos;
    }

    protected override float SetSpaceScale()
    {
        return GetComponent<RectTransform>().localScale.x * transform.parent.localScale.x;
    }
}
