
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Travel : BehaviorLeaf
{
    protected void GetAngleAndDist(Vector2 target, Vector3 self, out float angle, out float sqrDist)
    {
        Vector2 diff = new Vector2(target.x - self.x, target.y - self.y);
        angle = Vector2.SignedAngle(Vector2.up, diff);
        sqrDist = diff.sqrMagnitude;
    }

    protected void RotateTowards(float targetAngle, float shipAngle, BehaviorState state)
    {
        state.queuedRotation = LimitMagnitude(SignAngle((shipAngle - targetAngle)) / 90, 1);
        //Debug.Log(string.Format("{0} {1}", state.queuedRotation, shipAngle - targetAngle));
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
