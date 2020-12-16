using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyInfo : Singleton<GalaxyInfo>, IWindow
{
    private Canvas canvas;

    private GalaxyEdgeDict edgeDict;
    private List<GalaxyMapVertex> sectors;

    [SerializeField]
    private float radius;
    [SerializeField]
    private float percentInner;

    private static Vector2 direction = new Vector2(1, 1).normalized;

    private GalaxyMapVertex highlighted;

    protected override void Awake()
    {
        base.Awake();
        edgeDict = GetComponentInChildren<GalaxyEdgeDict>();
        sectors = GetComponentsInChildren<GalaxyMapVertex>().ToList();
        canvas = GetComponent<Canvas>();
        RectTransform rect = GetComponent<RectTransform>();
        Hide();
    }

    public float GetRadius()
    {
        return radius;
    }

    public float GetPercentInner()
    {
        return percentInner;
    }

    public Vector2 GetDirectionVector()
    {
        return direction;
    }

    public bool IsShown()
    {
        return canvas.enabled;
    }

    public void Show()
    {
        canvas.enabled = true;
        CenterOnVertex();
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    private void CenterOnVertex()
    {
        float scale;
        try
        {
            scale = 1 / highlighted.GetSpaceScale() * 3;
            scale = Mathf.Min(3, scale);
        }
        catch (System.DivideByZeroException e)
        {
            Debug.Log("Attempted to divide highlighted scale of 0 " + highlighted.GetSectorName() + " " + e);
            scale = 1;
        }
        transform.localPosition = -highlighted.GetSpacePosition() * scale * 35;
        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void HighlightLocation(GalaxyMapVertex location)
    {
        if (highlighted != null)
        {
            highlighted.Unhighlight();
        }
        location.Highlight();
        highlighted = location;
        CenterOnVertex();
    }

    public List<GalaxyMapVertex> GetSectors()
    {
        return sectors;
    }

    public GalaxyEdgeDict GetGalaxyEdgeDict()
    {
        return edgeDict;
    }
}
