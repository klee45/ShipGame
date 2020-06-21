using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyMapEdge : MonoBehaviour
{
    [SerializeField]
    private GalaxyMapVertex start;
    [SerializeField]
    private GalaxyMapVertex end;

    public void Setup(GalaxyMapVertex start, GalaxyMapVertex end)
    {
        this.start = start;
        this.end = end;
    }

    public GalaxyMapVertex GetStart()
    {
        return start;
    }

    public GalaxyMapVertex GetEnd()
    {
        return end;
    }

    public GalaxyMapEdge CreateOpposite(GameObject obj)
    {
        GalaxyMapEdge opposite = obj.AddComponent<GalaxyMapEdge>();
        opposite.Setup(end, start);
        return opposite;
    }

    public bool Contains(GalaxyMapVertex sector, out GalaxyMapVertex other)
    {
        if (sector == start)
        {
            other = end;
            return true;
        }
        else if (sector == end)
        {
            other = start;
            return true;
        }
        else
        {
            other = null;
            return false;
        }
    }
}
