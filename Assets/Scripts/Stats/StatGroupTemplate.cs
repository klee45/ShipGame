using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatGroupTemplate : MonoBehaviour
{
    public abstract StatGroup CreateGroup(GameObject attachee);
    public abstract float GetValue(float duration);
}
