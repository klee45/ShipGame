using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForCooldownAndEnergy : BehaviorLeaf
{
    [SerializeField]
    private float buffer;
    private bool waiting = false;
    private Timer timer;

    private void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
    }

    protected override string GetName()
    {
        return "Wait for cooldown and energy";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (!waiting)
        {
            waiting = true;
            if (state.ship.TryGetWeapon(state.weaponInfo.weaponIndex, out AWeapon weapon))
            {
                float energyTime = state.ship.GetEnergySystem().GetTimeToRecharge(weapon.GetEnergyCost());
                float cooldownTime = weapon.GetCooldownTimer().GetTime();
            }
        }
        return WaitHelper(state);
    }

    private NodeState WaitHelper(BehaviorState state)
    {
        var timeScale = state.ship.GetTimeScale();
        if (timer.Tick(TimeController.DeltaTime(timeScale)))
        {
            waiting = false;
            return NodeState.Success;
        }
        else
        {
            return NodeState.Running;
        }
    }
}
