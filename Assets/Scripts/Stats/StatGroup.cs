using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatGroupTemplate : Template<StatGroup, GameObject>
{
    public abstract float GetValue(float duration);
}

public abstract class StatGroup : MonoBehaviour
{
    public abstract void Tick(float scale);
    public abstract float GetValue();
}