using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionState : MonoBehaviour
{
    public Vector2 target;
    public float queuedVelocity = 0;
    public float queuedRotation = 0;

    public bool fireWeapon = false;
    public int weaponChoice = 0;
}
