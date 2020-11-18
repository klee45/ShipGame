using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEachAxis : ScaleInfo
{
    [SerializeField]
    private SizeMod xScale;
    [SerializeField]
    private SizeMod yScale;

    public override Vector3 Scale(Vector3 v)
    {
        return new Vector3(v.x * xScale.ToFloat(), v.y * yScale.ToFloat(), v.z);
    }
}
