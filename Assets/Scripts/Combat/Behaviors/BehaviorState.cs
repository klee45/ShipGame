using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SetTargetByPilotStats;

public class BehaviorState : MonoBehaviour
{
    public Ship ship;
    private DetectionShip shipDetections;
    public TargetInfo targetInfo;
    public WeaponInfo weaponInfo;
    public MovementInfo movementInfo;

    private void Start()
    {
        ship = GetComponentInParent<Ship>();
        shipDetections = ship.GetComponentInChildren<DetectionShip>();
        targetInfo = new TargetInfo();
        weaponInfo = new WeaponInfo();
        movementInfo = new MovementInfo();
    }

    public DetectionShip GetShipDetections()
    {
        return shipDetections;
    }

    public void ResetMovement()
    {
        movementInfo.queuedVelocity = 0;
        movementInfo.queuedRotation = 0;
        movementInfo.isBreaking = false;
    }

    public void ResetWeapons()
    {
        weaponInfo.fireWeapon = false;
        weaponInfo.weaponIndex = 0;
    }

    public class MovementInfo
    {
        public float queuedVelocity = 0;
        public float queuedRotation = 0;
        public bool isBreaking = false;
    }

    public class WeaponInfo
    {
        public bool fireWeapon = false;
        public int weaponIndex = 0;
        public int shotsRemaining = 0;
    }

    public class TargetInfo
    {
        public Ship ship;
        public Vector2 position;
        public float angleDiff;
        public float sqrDistDiff;
        public TargetType targetType;
    }
}
