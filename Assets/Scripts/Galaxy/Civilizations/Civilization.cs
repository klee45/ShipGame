using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Civilization : MonoBehaviour
{
    private void Start()
    {
        GalaxyInfo info = GalaxyInfo.instance;
        foreach (GalaxyMapVertex vertex in GetComponentsInChildren<GalaxyMapVertex>())
        {
            vertex.SetupMap(SetupVertex(vertex, info));
        }
    }

    protected abstract VertexDivision SetupVertex(GalaxyMapVertex vertex, GalaxyInfo info);

    protected float GetScoreFromAngle(GalaxyMapVertex vertex, GalaxyInfo info)
    {
        Vector2 pos = vertex.GetSpacePosition();
        Vector2 galaxyDirection = info.GetDirectionVector();

        float galaxyZero = galaxyDirection.ToAngle();
        float angle = pos.ToAngle();
        float angleDiff = Math.Mod(galaxyZero - angle, Mathf.PI * 2).ToDegree();
        if (angleDiff > 180)
        {
            angleDiff -= 360;
        }
        return Mathf.Abs(angleDiff) / 180;
    }
}
