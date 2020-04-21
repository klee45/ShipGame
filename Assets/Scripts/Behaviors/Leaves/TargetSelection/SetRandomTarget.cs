using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomTarget : BehaviorLeaf
{
    [SerializeField]
    private SelectType type = SelectType.ENEMY;

    private enum SelectType
    {
        SAME_TEAM,
        ENEMY
    }

    protected override string GetName()
    {
        return "Set random target";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        DetectionShip detections = state.GetShipDetections();
        if (detections.IsScanning())
        {
            Debug.Log("Waiting");
            return NodeState.RUNNING;
        }
        else
        {
            //Debug.Log(string.Format("Num detections: {0}", detections.Count()));
            bool result = false;
            int shipLayer = state.ship.gameObject.layer;
            switch (type)
            {
                case SelectType.ENEMY:
                    result = detections.GetRandomBlacklist(ref state.target.ship, shipLayer);
                    break;
                case SelectType.SAME_TEAM:
                    result = detections.GetRandomWhitelist(ref state.target.ship, shipLayer);
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
}
