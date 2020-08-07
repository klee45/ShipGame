using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAngleAndSqrDist : BehaviorLeaf
{
    private float angleLast;
    private float distLast;

    protected override string GetName()
    {
        return string.Format("Find angle and sqr-dist\n{0:#} {1:#}", angleLast, distLast);
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (state.target != null)
        {
            Vector2 target = state.target.position;
            Vector2 self = state.ship.transform.position;

            Vector2 diff = new Vector2(target.x - self.x, target.y - self.y);
            distLast = diff.sqrMagnitude;

            float angle = Vector2.SignedAngle(Vector2.up, diff);
            angleLast = GetAngleDiff(angle, state.ship.transform.rotation.eulerAngles.z);
            
            state.target.angleDiff = angleLast;
            state.target.sqrDistDiff = distLast;
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
