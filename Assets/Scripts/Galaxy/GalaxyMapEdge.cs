using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GalaxyEdgeDict;

public class GalaxyMapEdge : MonoBehaviour
{
    private GalaxyMapVertex start;
    private GalaxyMapVertex end;

    public void Setup(GalaxyMapVertexObjInfo startVertexInfo, GalaxyMapVertexObjInfo endVertexInfo)
    {
        this.start = startVertexInfo.vertex;
        this.end = endVertexInfo.vertex;
        this.name = string.Format("Edge from {0} to {1}", start.GetSectorID(), end.GetSectorID());

        RectTransform startRect = start.GetComponent<RectTransform>();
        RectTransform endRect = end.GetComponent<RectTransform>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector3 startPos = startVertexInfo.vertex.GetSpacePosition();
        Vector3 endPos = endVertexInfo.vertex.GetSpacePosition();
        //Debug.Log(startPos.ToString() + " -> " + endPos.ToString());
        Vector2 difference = endPos - startPos;
        float angle = Mathf.Atan2(difference.y, difference.x);
        //float length = difference.magnitude;

        //rectTransform.anchoredPosition = (startPos + endPos) / 2;
        //length -= (startRect.sizeDelta.magnitude + endRect.sizeDelta.magnitude) / 2;

        float startRadius = SetRadius(startRect, startVertexInfo);
        float endRadius = SetRadius(endRect, endVertexInfo);
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

    private float SetRadius(RectTransform rect, GalaxyMapVertexObjInfo info)
    {
        float val = rect.rect.width * rect.localScale.x / 2;
        if (info.isSubsector)
        {
            return val * info.vertex.transform.parent.localScale.x;
        }
        else
        {
            return val / 1.25f;
        }
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
