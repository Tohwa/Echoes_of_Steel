using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode
{
    public abstract NodeState Evaluate();
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}