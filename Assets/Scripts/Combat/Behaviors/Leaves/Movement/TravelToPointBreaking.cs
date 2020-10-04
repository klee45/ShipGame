using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPointBreaking : Travel
{
    [SerializeField]
    private bool debug = false;

    [SerializeField]
    private float successDistance;
    [SerializeField]
    private float slowAngle;

    protected override string GetName()
    {
        return "Travel to target point by reversing";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        RotateTowardsTargetAngleDiff(state);
        float sqrDiff = state.targetInfo.sqrDistDiff - successDistance * successDistance;
        if (sqrDiff > 0)
        {
            if (Mathf.Abs(state.targetInfo.angleDiff) > slowAngle)
            {
                state.movementInfo.queuedVelocity = -1.0f;
            }
            else
            {
                state.movementInfo.queuedVelocity = 1.0f;
            }

            return NodeState.Running;
        }
        else
        {
            return NodeState.Success;
        }
    }
}
