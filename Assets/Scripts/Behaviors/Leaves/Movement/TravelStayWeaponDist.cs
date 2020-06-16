using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelStayWeaponDist : Travel
{
    [SerializeField]
    private float minPercent = 0.75f;
    [SerializeField]
    private float maxPercent = 0.9f;
    [SerializeField]
    private Arsenal arsenal;

    private string nodeName;

    public void Awake()
    {
        nodeName = string.Format("Travel within {0} - {1}% of weapon range", minPercent, maxPercent);
    }
    
    protected override string GetName()
    {
        return nodeName;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        float range = arsenal.GetWeapon(state.weaponChoice).GetRange();
        float minRange = range * minPercent;
        float maxRange = range * maxPercent;
        return StayWithinHelper(state, minRange, maxRange);
    }
}
