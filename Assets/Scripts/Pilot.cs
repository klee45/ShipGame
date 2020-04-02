using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pilot : MonoBehaviour
{
    protected ActionAttack attack;
    protected ActionMovement move;
    protected ActionRotate rotate;

    public abstract void MakeDecisions(Ship ship);

    protected virtual void Rotate(Ship ship, float val)
    {
        ship.GetMovementStats().GetRotationStatGroup().Tick(val);
    }

    protected virtual void Move(Ship ship, float val)
    {
        ship.GetMovementStats().GetVelocityStatGroup().Tick(val);
    }
}
