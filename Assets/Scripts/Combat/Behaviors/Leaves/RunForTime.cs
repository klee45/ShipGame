using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunForTime : BehaviorLeaf
{
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private NodeState endState;

    private bool ready;

    private void Start()
    {
        ready = true;
        timer.OnComplete += () =>
        {
            ready = true;
            timer.RestartAndHalt();
        };
    }

    public override void Tick(float deltaTime)
    {
        timer.Tick(deltaTime);
    }

    protected override string GetName()
    {
        return string.Format("Run for {0}\n{1}", timer.GetMaxTime(), timer.GetTime());
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (ready)
        {
            ready = false;
            timer.TurnOn();
            return endState;
        }
        return NodeState.Running;
    }
}
