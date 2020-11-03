using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyMapEdge : MonoBehaviour
{
    private GalaxyMapVertex start;
    private GalaxyMapVertex end;

    public void Setup(GalaxyMapVertex start, GalaxyMapVertex end)
    {
        this.start = start;
        this.end = end;
        this.name = string.Format("Edge from {0} to {1}", start.GetSectorID(), end.GetSectorID());

        Vector3 startPos = start.GetComponent<RectTransform>().anchoredPosition;
        Vector3 endPos = end.GetComponent<RectTransform>().anchoredPosition;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = (startPos + endPos) / 2;
        Vector2 length = endPos - startPos;
        float angle = Mathf.Atan2(length.y, length.x);
        rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle);
        rectTransform.sizeDelta = new Vector2(length.magnitude, 0.1f);
        rectTransform.localScale = Vector3.one;
    }

    /*
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
    */
}
