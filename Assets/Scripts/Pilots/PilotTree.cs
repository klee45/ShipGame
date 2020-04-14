using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PilotTree : Pilot
{
    [SerializeField]
    private DecisionNode decisionTree;
    private DecisionState decisionState;

    private Ship ship;

    protected override void Awake()
    {
        base.Awake();
        decisionState = gameObject.AddComponent<DecisionState>();
    }

    protected override void GetComponentEntity()
    {
        ship = GetComponentInParent<Ship>();
    }

    public override void MakeActions()
    {
        decisionTree.UpdateState(decisionState);
        Rotate(ship, decisionState.queuedVelocity);
        Move(ship, decisionState.queuedRotation);
        if (decisionState.fireWeapon)
        {
            FireWeapon(ship, decisionState.weaponChoice);
        }
    }
}
