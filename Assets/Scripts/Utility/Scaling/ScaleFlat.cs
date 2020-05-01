using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFlat : ScaleInfo
{
    [SerializeField]
    private float scale = 1;

    public override Vector3 Scale(Vector3 v)
    {
        return v * scale;
    }
}
