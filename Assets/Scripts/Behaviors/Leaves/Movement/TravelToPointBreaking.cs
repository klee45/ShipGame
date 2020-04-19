using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPointBreaking : Travel
{
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

        float sqrDiff = state.target.sqrDistDiff - successDistance * successDistance;
        if (sqrDiff > 0)
        {
            if (Mathf.Abs(state.target.angleDiff) > slowAngle)
            {
                state.queuedVelocity = -1.0f;
            }
            else
            {
                state.queuedVelocity = 1.0f;
            }

            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
