using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorState : MonoBehaviour
{
    public Ship ship;
    public TargetInfo target;

    public float queuedVelocity = 0;
    public float queuedRotation = 0;
    public bool isBreaking = false;

    public bool fireWeapon = false;
    public int weaponChoice = 0;

    private void Start()
    {
        target = new TargetInfo();
    }

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

    public class TargetInfo
    {
        public Vector2 position;
        public float angleDiff;
        public float sqrDistDiff;
    }
}
