using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelStayWeaponDist : Travel
{
    [SerializeField]
    private float minPercent = 0.75f;
    [SerializeField]
    private float maxPercent = 0.9f;

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
        // Debug.Log(arsenal);
        // Debug.Log(arsenal.GetWeapon(state.weaponChoice));
        // Debug.Log(arsenal.GetWeapon(state.weaponChoice).GetRange());
        int slot = state.weaponInfo.weaponIndex;
        if (slot >= 0 && state.ship.GetArsenal().TryGetWeaponAtSlot(slot, out AWeapon weapon))
        {
            float range = weapon.GetRange();
            float minRange = range * minPercent;
            float maxRange = range * maxPercent;
            return StayWithinHelper(state, minRange, maxRange);
        }
        else
        {
            // Default values for no movement
            return StayWithinHelper(state, 0, 1000);
        }
    }
}
