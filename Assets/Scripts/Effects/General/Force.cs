using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : GeneralEffect, GeneralEffect.IMovementEffect, GeneralEffect.ITickEffect
{
    [SerializeField]
    private Vector2 force;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool isRelative = true;
    private float percent = 1.0f;

    public void Setup(Vector2 force, float duration, bool isRelative)
    {
        this.force = force;
        this.duration = duration;
        this.isRelative = isRelative;
    }

    public void Tick()
    {
        if (percent > 0)
        {
            percent -= (1 / duration) * Time.deltaTime;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void AddToHelper(EffectDict dict)
    {
        dict.movementEffects.Add(this);
        dict.tickEffects.Add(this);
    }

    protected override void RemoveFromHelper(EffectDict dict)
    {
        dict.movementEffects.Remove(this);
        dict.tickEffects.Remove(this);
    }

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