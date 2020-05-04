using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathModAdd : MathMod
{
    [SerializeField]
    private float add;

    public override float Apply(float val)
    {
        return val + add;
    }
}
