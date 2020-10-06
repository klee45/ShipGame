using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetTargetByPilotStats : BehaviorLeaf
{
    private const int MIN_DISTANCE_FOR_CLOSE_SQR = 10 * 10;

    protected override string GetName()
    {
        return "Set target by pilot stats";
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        state.GetShipDetections().SetupShips();
        //Debug.Log("Setting target");
        //Debug.Log(state.GetShipDetections().GetCount());
        PilotStats stats = state.ship.GetPilot().GetStats();
        if (FindTarget(state, stats, state.GetShipDetections(), out Ship targetShip))
        {
            //Debug.Log("Set target success " + targetShip);
            state.targetInfo.ship = targetShip;
            state.GetShipDetections().ResetRange();
            return NodeState.Success;
        }
        else
        {
            //Debug.Log("Set target fail");
            return NodeState.Failure;
        }
    }

    private bool FindTarget(BehaviorState state, PilotStats stats, DetectionShip detections, out Ship target)
    {
        // https://www.wolframalpha.com/input/?i=%283+-+x%29+%3B+%285+-+x%29+%3B+%28x+%2B+1%29%3B+%28x+-+1%29+%3B+in+range%281%2C+5%29
        float aggression = stats.GetAggression();
        float closeEnemyScore = 3 - aggression;
        float farEnemyScore = 5 - aggression;
        float closeAllyScore = aggression - 1;
        float farAllyScore = aggression;

        TargetPair[] pairs = new TargetPair[]
        {
            new TargetPair(closeEnemyScore, TargetType.CloseEnemy),
            new TargetPair(farEnemyScore, TargetType.FarEnemy),
            new TargetPair(closeAllyScore, TargetType.CloseAlly),
            new TargetPair(farAllyScore, TargetType.FarAlly)
        };

        // Sort array by ascending order
        pairs = pairs.OrderBy(pair => pair.GetValue()).ToArray();

        DetectionShip.DetectedMostImportant allies = detections.GetDetectedAllies();
        DetectionShip.DetectedMostImportant enemies = detections.GetDetectedEnemies();

        float distSqr;
        foreach (TargetPair pair in pairs)
        {
            state.targetInfo.targetType = pair.GetTargetType();
            switch (pair.GetTargetType())
            {
                case TargetType.CloseEnemy:
                    Ship closeEnemyShip = enemies.distanceSqr.GetLowest(out distSqr);
                    //Debug.Log(closeEnemyShip);
                    //Debug.Log(distSqr);
                    if (distSqr < MIN_DISTANCE_FOR_CLOSE_SQR || closeEnemyShip == null)
                    {
                        break;
                    }
                    else
                    {
                        target = closeEnemyShip;
                        return true;
                    }
                case TargetType.FarEnemy:
                    Ship farEnemyShip = enemies.distanceSqr.GetHighest(out distSqr);
                    //Debug.Log(farEnemyShip);
                    //Debug.Log(distSqr);
                    if (farEnemyShip == null)
                    {
                        break;
                    }
                    else
                    {
                        target = farEnemyShip;
                        return true;
                    }
                case TargetType.CloseAlly:
                    Ship closeAllyShip = allies.distanceSqr.GetLowest(out distSqr);
                    //Debug.Log(closeAllyShip);
                    //Debug.Log(distSqr);
                    if (distSqr < MIN_DISTANCE_FOR_CLOSE_SQR || closeAllyShip == null)
                    {
                        break;
                    }
                    else
                    {
                        target = closeAllyShip;
                        return true;
                    };
                case TargetType.FarAlly:
                    Ship farAllyShip = allies.distanceSqr.GetHighest(out distSqr);
                    //Debug.Log(farAllyShip);
                    //Debug.Log(distSqr);
                    if (farAllyShip == null)
                    {
                        break;
                    }
                    else
                    {
                        target = farAllyShip;
                        return true;
                    }
            }
            //Debug.Log(pair.b + " failed");
        }
        target = null;
        return false;
    }

    private class TargetPair : Pair<float, TargetType>
    {
        public TargetPair(float a, TargetType b) : base(a, b)
        {
        }
        public float GetValue() { return a; }
        public TargetType GetTargetType() { return b; }
    }

    public enum TargetType
    {
        CloseEnemy,
        CloseAlly,
        FarEnemy,
        FarAlly
    }
}
