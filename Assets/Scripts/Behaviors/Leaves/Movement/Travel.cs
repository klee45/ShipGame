
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Travel : BehaviorLeaf
{
    protected void RotateTowardsTargetAngleDiff(BehaviorState state)
    {
        state.queuedRotation = LimitMagnitude(state.target.angleDiff / 90f, 1f);
        // Debug.Log(string.Format("Diff: {0}", angleDiff));
    }

    protected float LimitMagnitude(float val, float limit)
    {
        if (val < 0)
        {
            return -Mathf.Min(-val, limit);
        }
        else
        {
            return Mathf.Min(val, limit);
        }
    }
}
