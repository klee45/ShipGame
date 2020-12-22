using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationOuter : Civilization
{
    protected override VertexDivision SetupVertex(GalaxyMapVertex vertex, GalaxyInfo info)
    {
        float empireScore = GetScoreFromAngle(vertex, info);
        
        Debug.Log(vertex.name + "  \t" + empireScore);

        return new VertexDivisionOuter(empireScore, 1 - empireScore);

        /*
        if (angleDiff.ToDegree() < 0)
        {
            Debug.Log(vertex.name + " :\t" + angleDiff.ToDegree() + " -------");
        }
        else
        {
            Debug.Log(vertex.name + " :\t" + angleDiff.ToDegree());
        }
        */

        /*
        float avalon = 1 - Mathf.Min(1, pos.magnitude / (info.GetRadius() * info.GetPercentInner()));
        Debug.Log(name + "\n" + pos + "\n" + Vector2.Dot(pos, direction));
        float federation = (1 - avalon) * (1 + (Vector2.Dot(pos, direction) / info.GetRadius())) / 2f;
        float empire = 1 - federation - avalon;
        */
    }
}
