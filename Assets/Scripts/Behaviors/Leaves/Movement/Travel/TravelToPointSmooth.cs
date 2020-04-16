using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPointSmooth : Travel
{
    private static string text = "Travel to target point smoothly";
    [SerializeField]
    private float successDistance;
    [SerializeField]
    private float slowDist;
    [SerializeField]
    private float speedMod;

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
            float sqrSlow = slowDist * slowDist;
            float divMod = sqrSlow * (1 + speedMod * Mathf.Abs(state.queuedRotation));
            state.queuedVelocity = Mathf.Min(sqrDist / divMod, 1);
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
