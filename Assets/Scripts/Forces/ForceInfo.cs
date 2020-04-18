using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceInfo : MonoBehaviour
{
    [SerializeField]
    public Vector2 force;
    [SerializeField]
    public float duration;
    [SerializeField]
    public float percent = 1.0f;
    [SerializeField]
    public bool isRelative = true;

    public float GetRange(float duration)
    {
        float time = Mathf.Min(this.duration, duration);
        return force.y * (time - (time * time / 2.0f));
    }
}
