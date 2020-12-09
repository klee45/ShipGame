using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SetTargetByPilotStats;

public class SelectWeaponByTargetDistance : BehaviorLeaf
{
    protected override string GetName()
    {
        return "Select weapon based on target distance";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        Ship target = state.targetInfo.ship;
        TargetType targetType = state.targetInfo.targetType;

        Arsenal arsenal = state.ship.GetArsenal();
        List<AWeapon> allWeapons = arsenal.GetAllWeapons();
        List<int> slots = arsenal.GetAllWeaponsPairedSlots();

        List<WeaponPair> preferredWeapons = new List<WeaponPair>();
        List<WeaponPair> secondaryWeapons = new List<WeaponPair>();
        for (int pos = 0; pos < allWeapons.Count; pos++)
        {
            AWeapon weapon = allWeapons[pos];
            int slot = slots[pos];
            TargetType preferred = weapon.GetPreferredTarget();
            TargetType secondary = weapon.GetSecondaryTarget();
            if (state.ship.CanFireWeapon(slot))
            {
                if (preferred == targetType)
                {
                    preferredWeapons.Add(new WeaponPair(weapon, slot));
                }
                else if (secondary == targetType)
                {
                    secondaryWeapons.Add(new WeaponPair(weapon, slot));
                }
            }
        }

        //Debug.LogWarning("Target type " + targetType.ToString());
        //Debug.LogWarning("Num weapons to choose from " + allWeapons.Count);
        //Debug.LogWarning("Num preferred : " + preferredWeapons.Count);
        //Debug.LogWarning("Num secondary : " + secondaryWeapons.Count);

        if (TrySet(state, preferredWeapons))
        {
            return NodeState.Success;
        }
        else if (TrySet(state, secondaryWeapons))
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }

    private bool TrySet(BehaviorState state, List<WeaponPair> weapons)
    {
        if (weapons.Count == 0)
        {
            return false;
        }
        else if (weapons.Count == 1)
        {
            WeaponPair weapon = weapons.First();
            //Debug.LogError("Set " + weapon.a + " at " + weapon.b);
            SetupWeapon(state, weapon);
            return true;
        }
        else
        {
            WeaponPair weapon = weapons.GetRandomElement();
            //Debug.LogError("Set " + weapon.a + " at " + weapon.b);
            SetupWeapon(state, weapon);
            return true;
        }
    }

    private void SetupWeapon(BehaviorState state, WeaponPair pair)
    {
        state.weaponInfo.weaponIndex = pair.GetSlot();
        state.weaponInfo.shotsRemaining = GetSuggestedShots(pair.GetWeapon());
    }

    private int GetSuggestedShots(AWeapon weapon)
    {
        int min = weapon.GetMinSuggestedShots();
        int max = weapon.GetMaxSuggestedShots();
        return Random.Range(min, max + 1);
    }

    private class WeaponPair : Pair<AWeapon, int>
    {
        public WeaponPair(AWeapon a, int b) : base(a, b)
        {
        }

        public AWeapon GetWeapon() { return a; }
        public int GetSlot() { return b; }
    }
}
