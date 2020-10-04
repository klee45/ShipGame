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

        AWeapon[] allWeapons = state.ship.GetArsenal().GetWeapons();

        List<WeaponPair> preferredWeapons = new List<WeaponPair>();
        List<WeaponPair> secondaryWeapons = new List<WeaponPair>();
        for (int pos = 0; pos < allWeapons.Length; pos++)
        {
            AWeapon weapon = allWeapons[pos];
            TargetType preferred = weapon.GetPreferredTarget();
            TargetType secondary = weapon.GetSecondaryTarget();
            if (state.ship.CanFireWeapon(pos))
            {
                if (preferred == targetType)
                {
                    preferredWeapons.Add(new WeaponPair(weapon, pos));
                }
                else if (secondary == targetType)
                {
                    secondaryWeapons.Add(new WeaponPair(weapon, pos));
                }
            }
        }

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
            SetupWeapon(state, weapons.First());
            return true;
        }
        else
        {
            SetupWeapon(state, weapons[Random.Range(0, weapons.Count)]);
            return true;
        }
    }

    private void SetupWeapon(BehaviorState state, WeaponPair pair)
    {
        state.weaponInfo.weaponIndex = pair.GetPos();
        state.targetInfo.shotsRemaining = GetSuggestedShots(pair.GetWeapon());
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
        public int GetPos() { return b; }
    }
}
