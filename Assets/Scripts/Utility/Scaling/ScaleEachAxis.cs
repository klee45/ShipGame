﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEachAxis : ScaleInfo
{
    [SerializeField]
    private SizeModNumber xScale;
    [SerializeField]
    private SizeModNumber yScale;

    public override Vector3 Scale(Vector3 v)
    {
        return new Vector3(v.x * xScale.ToFloat(), v.y * yScale.ToFloat(), v.z);
    }

    public override Vector3 GetVector3()
    {
        return new Vector3(xScale.ToFloat(), yScale.ToFloat(), 1);
    }

    public override float GetY()
    {
        return yScale.ToFloat();
    }
}