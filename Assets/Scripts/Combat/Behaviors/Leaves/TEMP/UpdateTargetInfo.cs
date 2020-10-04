using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTargetInfo : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Update target info";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (state.targetInfo != null)
        {
            state.targetInfo.position = state.targetInfo.ship.transform.position;
            Vector2 target = state.targetInfo.position;
            Vector2 self = state.ship.transform.position;

            Vector2 vectorDiff = new Vector2(target.x - self.x, target.y - self.y);
            float sqrDistLast = vectorDiff.sqrMagnitude;

            float angle = Vector2.SignedAngle(Vector2.up, vectorDiff);
            float angleLast = GetAngleDiff(angle, state.ship.transform.rotation.eulerAngles.z);

            state.targetInfo.angleDiff = angleLast;
            state.targetInfo.sqrDistDiff = sqrDistLast;

            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }

    private float GetAngleDiff(float targetAngle, float shipAngle)
    {
        return SignAngle((shipAngle - targetAngle));
    }

    private float SignAngle(float angle)
    {
        if (angle > 180)
        {
            return angle - 360;
        }
        else
        {
            return angle;
        }
    }
}
