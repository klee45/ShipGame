using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MathMod : MonoBehaviour
{
    [SerializeField]
    private int priority;

    public abstract float Apply(float val);
    public int GetPriority()
    {
        return priority;
    }
}
