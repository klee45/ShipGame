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

    public static Force Create(GameObject target, ForceInfo info)
    {
        Force f = target.AddComponent<Force>();
        f.force = info.force;
        f.duration = info.duration;
        f.percent = info.percent;
        f.isRelative = info.isRelative;
        return f;
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
