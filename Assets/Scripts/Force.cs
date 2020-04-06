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

    private void Update()
    {
        percent -= decayPerSec * Time.deltaTime;
        if (percent <= 0)
        {
            Destroy(this);
        }
    }

    public Vector2 GetVector()
    {
        return force * percent;
    }

    public bool HasForce()
    {
        return percent > 0;
    }
}
