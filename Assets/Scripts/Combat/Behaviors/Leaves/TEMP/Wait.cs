using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : BehaviorLeaf
{
    [SerializeField]
    private NodeState resultState = NodeState.Success;
    [SerializeField]
    private float duration;

    private Timer timer;

    private void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(duration);
    }

    protected override string GetName()
    {
        return string.Format("Wait {0} seconds", duration);
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        var timeScale = state.ship.GetTimeScale();
        if (timer.Tick(TimeController.DeltaTime(timeScale)))
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Running;
        }
    }
}
