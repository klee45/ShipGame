using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFromTargetList : BehaviorLeaf
{
    [SerializeField]
    private TargetList targetList;

    private int pos = 0;
    private float startTime = 0;

    protected override string GetName()
    {
        return "Select from target list " + pos;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        if (pos == 0)
        {
            startTime = Time.time;
        }
        if (pos < targetList.targets.Count)
        {
            GameObject target = targetList.targets[pos];
            state.target.position = target.transform.position;
            pos++;
            return NodeState.Success;
        }
        else
        {
            Debug.Log("Time taken: " + (Time.time - startTime));
            state.ship.transform.position = targetList.targets[0].transform.position;
            state.ship.transform.localEulerAngles = Vector3.zero;
            pos = 0;
            return NodeState.Running;
        }
    }
}
