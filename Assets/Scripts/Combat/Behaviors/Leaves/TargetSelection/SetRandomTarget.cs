﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomTarget : BehaviorLeaf
{
    [SerializeField]
    private SelectType type = SelectType.Enemy;

    private enum SelectType
    {
        SameTeam,
        Enemy
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
            //Debug.Log("Waiting");
            return NodeState.Running;
        }
        else
        {
            //Debug.Log(string.Format("Num detections: {0}", detections.Count()));
            bool result = false;
            int shipLayer = state.ship.gameObject.layer;
            switch (type)
            {
                case SelectType.Enemy:
                    result = detections.GetRandomBlacklist(ref state.targetInfo.ship, shipLayer);
                    break;
                case SelectType.SameTeam:
                    result = detections.GetRandomWhitelist(ref state.targetInfo.ship, shipLayer);
                    break;
            }
            if (result)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }
        }
    }
}
