using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyInfo : Singleton<GalaxyInfo>
{
    private List<GalaxyMapVertex> sectors;
    private List<GalaxyMapEdge> hyperlanes;

    protected override void Awake()
    {
        base.Awake();
        sectors = GetComponentsInChildren<GalaxyMapVertex>().ToList();
        hyperlanes = GetComponentsInChildren<GalaxyMapEdge>().ToList();
        SetSpriteState(false);
    }

    private void Start()
    {
        int pos = 1;
        foreach (GalaxyMapVertex vertex in sectors)
        {
            string id = "1 - " + pos.ToString();
            string fullName = "Sector " + id;
            vertex.gameObject.name = fullName;
            vertex.GetComponentInChildren<Text>().text = id;
            vertex.SetSectorNameDebugging(fullName);
            pos++;
        }
    }

    private void SetSpriteState(bool enabled)
    {
        /*
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.enabled = enabled;
        }
        */
    }

    public List<GalaxyMapVertex> GetSectors()
    {
        return sectors;
    }

    public List<GalaxyMapEdge> GetHyperlanes()
    {
        return hyperlanes;
    }
}
