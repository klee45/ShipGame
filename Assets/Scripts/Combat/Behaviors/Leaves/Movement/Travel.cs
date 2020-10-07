
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Travel : BehaviorLeaf
{
    protected void RotateTowardsTargetAngleDiff(BehaviorState state)
    {
        float mag = LimitMagnitude(state.targetInfo.angleDiff / 20f, 1f, out int sign);
        /*
        float reverseMag = 1 - mag;
        float squared = reverseMag * reverseMag;
        state.queuedRotation = sign * 0.9f * (1.1f - squared);
        */
        state.movementInfo.queuedRotation = sign * 0.9f * (0.1f + mag * mag);
        //Debug.Log("Mag " + mag);
        //Debug.Log("Rot " + state.queuedRotation);
        // Debug.Log(string.Format("Diff: {0}", angleDiff));
    }

    protected float LimitMagnitude(float val, float limit, out int sign)
    {
        if (val < 0)
        {
            sign = -1;
            return Mathf.Min(-val, limit);
        }
        else
        {
            sign = 1;
            return Mathf.Min(val, limit);
        }
    }

    protected NodeState StayWithinHelper(BehaviorState state, float minDist, float maxDist)
    {
        RotateTowardsTargetAngleDiff(state);
        float targetDist = state.targetInfo.sqrDistDiff;
        if (targetDist > maxDist * maxDist)
        {
            state.movementInfo.queuedVelocity = 1.0f;
        }
        else if (targetDist < minDist * minDist)
        {
            state.movementInfo.queuedVelocity = -1.0f;
        }
        else
        {
            state.movementInfo.queuedVelocity = 0f;
        }
        return NodeState.Running;
    }
}
