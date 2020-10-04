using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PilotTree : APilot
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
        //PrintTreeSize(counts);
    }

    private void PrintTreeSize(int[] counts)
    {
        Debug.Log(string.Format("[{0}]", string.Join(" ", counts)));
    }

    public override void Tick(float deltaTime)
    {
        behaviorTree.Tick(deltaTime);
    }

    public Vector2 GetTargetPos()
    {
        return behaviorState.targetInfo.position;
    }

    public override void MakeActions()
    {
        BehaviorNode.NodeState rootState =  behaviorTree.UpdateState(behaviorState);
        //Debug.Log(behaviorState.fireWeapon);
        //Debug.Log(behaviorState.weaponChoice);
        Ship ship = behaviorState.ship;
        Rotate(ship, behaviorState.movementInfo.queuedRotation);
        Move(ship, behaviorState.movementInfo.queuedVelocity);
        if (behaviorState.weaponInfo.fireWeapon)
        {
            FireWeapon(ship, behaviorState.weaponInfo.weaponIndex);
            behaviorState.weaponInfo.fireWeapon = false;
        }
        switch(rootState)
        {
            case BehaviorNode.NodeState.Success:
            case BehaviorNode.NodeState.Failure:
                behaviorTree.ResetNode();
                break;
        }
    }
}
