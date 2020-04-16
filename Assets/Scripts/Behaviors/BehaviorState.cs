using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorState : MonoBehaviour
{
    public Ship ship;
    public Vector2 target;

    public float queuedVelocity = 0;
    public float queuedRotation = 0;
    public bool isBreaking = false;

    public bool fireWeapon = false;
    public int weaponChoice = 0;

    public void ResetMovement()
    {
        queuedVelocity = 0;
        queuedRotation = 0;
        isBreaking = false;
    }

    public void ResetWeapons()
    {
        fireWeapon = false;
        weaponChoice = 0;
    }
}
