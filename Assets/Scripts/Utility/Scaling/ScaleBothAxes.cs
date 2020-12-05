using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBothAxes : ScaleInfo
{
    [SerializeField]
    private SizeModNumber scale;

    public override Vector3 Scale(Vector3 v)
    {
        return v * scale.ToFloat();
    }
}
