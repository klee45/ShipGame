using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AForce : GeneralEffect, GeneralEffect.IMovementEffect
{
    [SerializeField]
    protected Vector2 force;
    [SerializeField]
    protected bool isRelative = true;

    public Vector3 GetMovement(float timeDelta)
    {
        if (isRelative)
        {
            //Debug.Log(transform.localEulerAngles);
            Vector3 result = transform.localRotation * force * timeDelta;
            return result;
        }
        else
        {
            return new Vector3(force.x * timeDelta, force.y * timeDelta, 0);
        }
    }
}