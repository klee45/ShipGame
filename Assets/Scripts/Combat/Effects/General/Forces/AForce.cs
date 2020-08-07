using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AForce : EntityEffect, EntityEffect.IMovementEffect
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
            return RelativeHelper(timeDelta);
        }
        else
        {
            return NotRelativeHelper(timeDelta);
        }
    }

    protected virtual Vector3 RelativeHelper(float timeDelta)
    {
        Vector3 result = transform.localRotation * force * timeDelta;
        return result;
    }

    protected virtual Vector3 NotRelativeHelper(float timeDelta)
    {
        return new Vector3(force.x * timeDelta, force.y * timeDelta, 0);
    }

    public static readonly EffectTag[] tags = new EffectTag[] { EffectTag.Force, EffectTag.Movement };
    public override EffectTag[] GetTags()
    {
        return tags;
    }
}