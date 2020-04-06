using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceInfo : MonoBehaviour
{
    [SerializeField]
    public Vector2 force;
    [SerializeField]
    public float decayPerSec;
    [SerializeField]
    public float percent = 1.0f;
    [SerializeField]
    public bool isRelative = true;
}
