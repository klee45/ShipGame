using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomTarget : BehaviorLeaf
{
    [SerializeField]
    private SelectType type = SelectType.ENEMY;

    private enum SelectType
    {
        ALLY,
        ENEMY
    }

    protected override string GetName()
    {
        return "Set random target";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        Detection detections = state.GetDetections();
        bool result = false;
        int shipLayer = state.ship.gameObject.layer;
        switch (type)
        {
            case SelectType.ENEMY:
                result = detections.GetMemoryDict().GetRandomBlacklist(ref state.target.ship, shipLayer);
                break;
            case SelectType.ALLY:
                result = detections.GetMemoryDict().GetRandomWhitelist(ref state.target.ship, shipLayer);
                break;
        }
        if (result)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
