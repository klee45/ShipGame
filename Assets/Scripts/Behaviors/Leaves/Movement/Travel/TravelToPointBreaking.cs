using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPointBreaking : Travel
{
    private static string text = "Travel to target point by reversing";
    [SerializeField]
    private float successDistance;
    [SerializeField]
    private float slowAngle;

    protected override string GetName()
    {
        return text;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state, Ship ship)
    {
        GetAngleAndDist(state.target, ship.transform.position, out float angle, out float sqrDist);
        RotateTowards(angle, ship.transform.rotation.eulerAngles.z, state);

        float sqrDiff = sqrDist - successDistance * successDistance;
        if (sqrDiff > 0)
        {
            if (Mathf.Abs(state.queuedRotation) > slowAngle)
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
