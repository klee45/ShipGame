using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatGroupTemplate : Template<StatGroup, GameObject>
{
    public abstract float GetRange(float duration);
    public abstract float GetDuration(float range);
}

public abstract class StatGroup : MonoBehaviour
{
    public abstract void Tick(float scale, float deltaTime);
    public abstract float GetValue();

    public abstract void MultMod(float inc, float dec);
    public abstract void MultModUndo(float inc, float dec);
}