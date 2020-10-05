using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasShotsRemaining : BehaviorConditional
{
    [SerializeField]
    private int minShotsRemaining = 1;

    protected override bool Conditional(BehaviorState state)
    {
        return state.weaponInfo.shotsRemaining >= minShotsRemaining;
    }

    protected override string GetName()
    {
        return "Has shots remaining";
    }
}
