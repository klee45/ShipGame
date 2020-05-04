using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathModMult : MathMod
{
    [SerializeField]
    private float mult;

    public override float Apply(float val)
    {
        return val * mult;
    }
}
