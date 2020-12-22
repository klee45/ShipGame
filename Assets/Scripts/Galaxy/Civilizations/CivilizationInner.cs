using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationInner : Civilization
{
    [SerializeField]
    private float percentInner;
    [SerializeField]
    private float minPercent;

    protected override VertexDivision SetupVertex(GalaxyMapVertex vertex, GalaxyInfo info)
    {
        float empireScore = GetScoreFromAngle(vertex, info);

        float dist = vertex.GetSpacePosition().magnitude;
        float maxDist = percentInner * info.GetRadius();
        float score = Mathf.Min(1, (maxDist - dist) * (1 + minPercent) / maxDist);
        float remainder = 1 - score;

        return new VertexDivisionInner(empireScore * remainder, (1 - empireScore) * remainder, score);
    }
}