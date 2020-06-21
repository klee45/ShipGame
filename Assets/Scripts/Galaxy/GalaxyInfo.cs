using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GalaxyInfo : Singleton<GalaxyInfo>
{
    private List<GalaxyMapVertex> sectors;
    private List<GalaxyMapEdge> hyperlanes;
    private List<SpriteRenderer> sprites;

    protected override void Awake()
    {
        base.Awake();
        sectors = GetComponentsInChildren<GalaxyMapVertex>().ToList();
        hyperlanes = GetComponentsInChildren<GalaxyMapEdge>().ToList();
        sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        SetSpriteState(false);
    }

    private void SetSpriteState(bool enabled)
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.enabled = enabled;
        }
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
