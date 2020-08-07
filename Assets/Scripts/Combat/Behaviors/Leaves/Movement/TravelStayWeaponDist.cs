using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelStayWeaponDist : Travel
{
    [SerializeField]
    private float minPercent = 0.75f;
    [SerializeField]
    private float maxPercent = 0.9f;
    private Arsenal arsenal;

    private string nodeName;

    public void Awake()
    {
        arsenal = GetComponentInParent<Ship>().GetComponentInChildren<Arsenal>();
        nodeName = string.Format("Travel within {0} - {1}% of weapon range", minPercent, maxPercent);
    }
    
    protected override string GetName()
    {
        return nodeName;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        // Debug.Log(arsenal);
        // Debug.Log(arsenal.GetWeapon(state.weaponChoice));
        // Debug.Log(arsenal.GetWeapon(state.weaponChoice).GetRange());
        float range = arsenal.GetWeapon(state.weaponChoice).GetRange();
        float minRange = range * minPercent;
        float maxRange = range * maxPercent;
        return StayWithinHelper(state, minRange, maxRange);
    }
}
