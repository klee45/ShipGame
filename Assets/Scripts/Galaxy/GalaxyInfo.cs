using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyInfo : Singleton<GalaxyInfo>
{
    private GalaxyEdgeDict edgeDict;
    private List<GalaxyMapVertex> sectors;

    protected override void Awake()
    {
        base.Awake();
        edgeDict = GetComponentInChildren<GalaxyEdgeDict>();
        sectors = GetComponentsInChildren<GalaxyMapVertex>().ToList();
        SetSpriteState(false);
    }

    private void Start()
    {
        /*
        int pos = 1;
        foreach (GalaxyMapVertex vertex in sectors)
        {
            string id = "1-" + pos.ToString();
            string fullName = "Sector " + id;
            vertex.gameObject.name = fullName;
            vertex.GetComponentInChildren<Text>().text = id;
            vertex.SetSectorNameDebugging(fullName);
            pos++;
        }
        */
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

    public GalaxyEdgeDict GetGalaxyEdgeDict()
    {
        return edgeDict;
    }
}
