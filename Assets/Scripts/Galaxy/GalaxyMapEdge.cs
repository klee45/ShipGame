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

        RectTransform startRect = start.GetComponent<RectTransform>();
        RectTransform endRect = end.GetComponent<RectTransform>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector3 startPos = startRect.anchoredPosition;
        Vector3 endPos = endRect.anchoredPosition;
        Debug.Log(startPos.ToString() + " -> " + endPos.ToString());
        Vector2 difference = endPos - startPos;
        float angle = Mathf.Atan2(difference.y, difference.x);
        //float length = difference.magnitude;

        //rectTransform.anchoredPosition = (startPos + endPos) / 2;
        //length -= (startRect.sizeDelta.magnitude + endRect.sizeDelta.magnitude) / 2;

        float startRadius = startRect.rect.width * startRect.localScale.x / 2;
        float endRadius = endRect.rect.width * endRect.localScale.x / 2;
        //Debug.Log(startRadius + ", " + endRadius);
        Vector3 leftPos = startPos + (angle.RadToVector2() * startRadius).ToVector3();
        Vector3 rightPos = endPos - (angle.RadToVector2() * endRadius).ToVector3();
        //Debug.Log(start.GetSectorName() + " -> " + end.GetSectorName() + " : " + leftPos.ToString() + " -> " + rightPos.ToString());
        float length = (rightPos - leftPos).magnitude - (startRadius + endRadius);

        rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle);
        rectTransform.anchoredPosition = (leftPos + rightPos) / 2;
        rectTransform.localScale = Vector3.one;
        //Debug.Log(length);
        rectTransform.sizeDelta = new Vector2((rightPos - leftPos).magnitude + 0.025f, 0.1f);
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
