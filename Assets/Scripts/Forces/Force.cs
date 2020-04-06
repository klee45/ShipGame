using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    [SerializeField]
    private Vector2 force;
    [SerializeField]
    private float decayPerSec;
    [SerializeField]
    private float percent = 1.0f;
    [SerializeField]
    private bool isRelative = true;

    public static Force Create(GameObject target, ForceInfo info)
    {
        Force f = target.AddComponent<Force>();
        f.force = info.force;
        f.decayPerSec = info.decayPerSec;
        f.percent = info.percent;
        f.isRelative = info.isRelative;
        return f;
    }

    private void Update()
    {
        if (percent > 0)
        {
            percent -= decayPerSec * Time.deltaTime;
        }        
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
