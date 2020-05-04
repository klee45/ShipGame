using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTemplate : GeneralEffectTemplate
{
    [SerializeField]
    private Vector2 force;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float percent = 1.0f;
    [SerializeField]
    private bool isRelative = true;

    protected override GeneralEffect CreateEffect(GameObject obj)
    {
        Force f = gameObject.AddComponent<Force>();
        f.Setup(force, duration, percent, isRelative);
        return f;
    }

    public override float GetRangeMod()
    {
        return Force.GetMovementHelper(duration, force);
    }
}

public class Force : GeneralEffect, GeneralEffect.IMovementEffect, GeneralEffect.ITickEffect
{
    private Vector2 force;
    private float duration;
    private float percent = 1.0f;
    private bool isRelative = true;

    public void Setup(Vector2 force, float duration, float percent, bool isRelative)
    {
        this.force = force;
        this.duration = duration;
        this.percent = percent;
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

    public float GetMovement(float duration)
    {
        return GetMovementHelper(duration, force);
    }

    public static float GetMovementHelper(float duration, Vector2 force)
    {
        return force.y * (duration - (duration * duration / 2.0f));
    }

    public override void AddTo(EffectDict e)
    {
        e.movementEffects.Add(this);
    }

    public Vector3 GetMovement()
    {
        if (isRelative)
        {
            //Debug.Log(transform.localEulerAngles);
            Vector3 result = transform.localRotation * force;
            return result;
        }
        else
        {
            return new Vector3(force.x, force.y, 0);
        }
    }
}