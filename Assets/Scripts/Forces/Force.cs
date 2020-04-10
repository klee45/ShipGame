using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    [SerializeField]
    private Vector2 force;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float percent = 1.0f;
    [SerializeField]
    private bool isRelative = true;

    public void Initialize(ForceInfo info)
    {
        force = info.force;
        duration = info.duration;
        percent = info.percent;
        isRelative = info.isRelative;
    }

    private void Update()
    {
        if (percent > 0)
        {   
            percent -= (1 / duration) * Time.deltaTime;
        }        
    }

    public float GetRange(float duration)
    {
        float time = Mathf.Min(this.duration, duration);
        return force.y * (time - (time * time / 2.0f));
    }

    public float GetPercent()
    {
        return percent;
    }

    public Vector2 GetVector()
    {
        return force * percent;
    }

    public bool HasForce()
    {
        return percent > 0;
    }

    public bool IsRelative()
    {
        return isRelative;
    }
}
