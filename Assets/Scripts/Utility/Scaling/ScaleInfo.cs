using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScaleInfo : MonoBehaviour
{
    public abstract Vector3 Scale(Vector3 v);
    public abstract Vector3 GetVector3();
    public abstract float GetY();
}