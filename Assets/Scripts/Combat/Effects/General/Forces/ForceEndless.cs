using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEndless : AForce
{
    public void Setup(Vector2 force, bool isRelative)
    {
        this.force = force;
        this.isRelative = isRelative;
    }

    public override void AddTo(EffectDict dict)
    {
        dict.movementEffects.Add(this, () => new ForceEndlessEffectCase(EffectDict.EffectCaseType.SINGLE));
    }

    public override string GetName()
    {
        return "Force";
    }

    private class ForceEndlessEffectCase : EffectDict.AMovementEffectCase<ForceEndless>
    {
        public ForceEndlessEffectCase(EffectDict.EffectCaseType type) : base(type)
        {
        }

        public override Vector3 GetMovement(float deltaTime)
        {
            Debug.LogWarning("Endless force has no range estimation");
            return Vector3.zero;
        }
    }
}