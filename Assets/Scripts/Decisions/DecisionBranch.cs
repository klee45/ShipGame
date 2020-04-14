using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionBranch : DecisionNode
{
    [SerializeField]
    private DecisionNode children;
}
