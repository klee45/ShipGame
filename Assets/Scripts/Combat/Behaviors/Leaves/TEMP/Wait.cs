using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : BehaviorLeaf
{
    [SerializeField]
    private float duration;
    private float currentTime;

    private void Awake()
    {
        Reset();
    }

    private void Reset(float overflow=0)
    {
        currentTime = overflow;
    }

    protected override string GetName()
    {
        return string.Format("Wait {0} seconds", duration);
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        var timeScale = state.ship.GetTimeScale();
        currentTime += TimeController.DeltaTime(timeScale);
        if (currentTime >= duration)
        {
            Reset(currentTime - duration);
            return NodeState.Success;
        }
        else
        {
            return NodeState.Running;
        }
    }
}
