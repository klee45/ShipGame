using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PilotTree : Pilot
{
    [SerializeField]
    private BehaviorNode behaviorTree;
    private BehaviorState behaviorState;

    protected void Awake()
    {
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

    public Vector2 GetTargetPos()
    {
        return behaviorState.target.position;
    }

    public override void MakeActions()
    {
        BehaviorNode.NodeState rootState =  behaviorTree.UpdateState(behaviorState);
        //Debug.Log(behaviorState.fireWeapon);
        //Debug.Log(behaviorState.weaponChoice);
        Ship ship = behaviorState.ship;
        Rotate(ship, behaviorState.queuedRotation);
        Move(ship, behaviorState.queuedVelocity);
        if (behaviorState.fireWeapon)
        {
            FireWeapon(ship, behaviorState.weaponChoice);
            behaviorState.fireWeapon = false;
        }
        switch(rootState)
        {
            case BehaviorNode.NodeState.SUCCESS:
            case BehaviorNode.NodeState.FAILURE:
                behaviorTree.ResetNode();
                break;
        }
    }
}
