using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStats : MonoBehaviour
{
    [SerializeField]
    private StatGroup rotation, velocity;

    public StatGroup GetRotationStatGroup() { return rotation; }
    public StatGroup GetVelocityStatGroup() { return velocity; }

    public float GetRotationValue() { return rotation.GetValue() * Mathf.Rad2Deg; }
    public float GetVelocityValue() { return velocity.GetValue(); }
}
