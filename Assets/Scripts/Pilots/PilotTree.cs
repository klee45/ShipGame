using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PilotTree : Pilot
{
    [SerializeField]
    private BehaviorNode behaviorTree;
    private BehaviorState behaviorState;

    private Ship ship;

    protected override void Awake()
    {
        base.Awake();
        behaviorState = gameObject.AddComponent<BehaviorState>();
    }

    private void Start()
    {
        int[] counts = behaviorTree.TraverseCount();
        PrintTreeSize(counts);
    }

    private void PrintTreeSize(int[] counts)
    {
        Debug.Log(string.Format("[{0}]", string.Join(" ", counts)));
    }

    protected override void GetComponentEntity()
    {
        ship = GetComponentInParent<Ship>();
    }

    public override void MakeActions()
    {
        BehaviorNode.NodeState rootState =  behaviorTree.UpdateState(behaviorState);
        //Debug.Log(behaviorState.fireWeapon);
        //Debug.Log(behaviorState.weaponChoice);
        Rotate(ship, behaviorState.queuedVelocity);
        Move(ship, behaviorState.queuedRotation);
        if (behaviorState.fireWeapon)
        {
            FireWeapon(ship, behaviorState.weaponChoice);
            behaviorState.fireWeapon = false;
        }
        switch(rootState)
        {
            case BehaviorNode.NodeState.SUCCESS:
            case BehaviorNode.NodeState.FAILURE:
                behaviorTree.Reset();
                break;
        }
    }
}
