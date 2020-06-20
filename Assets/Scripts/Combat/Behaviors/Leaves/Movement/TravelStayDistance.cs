using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelStayDistance : Travel
{
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;

    private string nodeName;

    public void Awake()
    {
        nodeName = string.Format("Travel within distance {0} - {1}", minDist, maxDist);
    }

    protected override string GetName()
    {
        return nodeName;
    }

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        return StayWithinHelper(state, minDist, maxDist);
    }
}
