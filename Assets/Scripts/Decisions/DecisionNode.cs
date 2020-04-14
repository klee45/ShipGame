using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionNode : MonoBehaviour
{
    public abstract NodeState UpdateState(DecisionState state);

    public enum NodeState
    {
        SUCCESS,
        RUNNING,
        FAILURE
    }
}
