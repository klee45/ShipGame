using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPointSmooth : Travel
{
    [SerializeField]
    private float successDistance;
    [SerializeField]
    private float slowDist;
    [SerializeField]
    private float speedMod;

    protected override string GetName()
    {
        return "Travel to target point smoothly";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        RotateTowardsTargetAngleDiff(state);

        float sqrDiff = state.targetInfo.sqrDistDiff - successDistance * successDistance;
        if (sqrDiff > 0)
        {
            float sqrSlow = slowDist * slowDist;
            float divMod = sqrSlow * (1 + speedMod * Mathf.Abs(state.movementInfo.queuedRotation));
            state.movementInfo.queuedVelocity = Mathf.Min(state.targetInfo.sqrDistDiff / divMod, 1);
            return NodeState.Running;
        }
        else
        {
            return NodeState.Success;
        }
    }
}
