using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePer : ScaleInfo
{
    [SerializeField]
    private Vector2 scale = Vector2.one;

    public override Vector3 Scale(Vector3 v)
    {
        return new Vector3(v.x * scale.x, v.y * scale.y, v.z);
    }
}
