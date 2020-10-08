using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinAngle : BehaviorConditional
{
    [SerializeField]
    private float angle;

    protected override bool Conditional(BehaviorState state)
    {
        return Mathf.Abs(state.targetInfo.angleDiff) < angle;
    }

    protected override string GetName()
    {
        return string.Format("Within {0} degrees", angle);
    }
}
