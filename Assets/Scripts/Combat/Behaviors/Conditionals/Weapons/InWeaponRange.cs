using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWeaponRange : BehaviorConditional
{
    [SerializeField]
    private float percent;

    private void Start()
    {
        if (percent < 0)
        {
            Debug.LogWarning("In weapon range can't be negative");
            percent = 0;
        }
    }

    protected override string GetName()
    {
        return string.Format("In weapon range\n{0}%", percent * 100);
    }

    protected override bool Conditional(BehaviorState state)
    {
        if (state.ship.TryGetWeapon(state.weaponInfo.weaponIndex, out AWeapon weapon))
        {
            float weaponRange = weapon.GetRange();
            return state.targetInfo.sqrDistDiff * percent * percent < weaponRange * weaponRange;
        }
        else
        {
            return false;
        }
    }
}
