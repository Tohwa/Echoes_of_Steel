using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTNode
{
    private List<BTNode> nodes;

    public BTSequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Evaluating BTSequence Node...");
        foreach (BTNode node in nodes)
        {
            NodeState nodeState = node.Evaluate();
            Debug.Log($"BTSequence Node {node.GetType().Name} evaluated with state: {nodeState}");
            if (nodeState == NodeState.FAILURE)
            {
                return NodeState.FAILURE;
            }
            if (nodeState == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
        }
        return NodeState.SUCCESS;
    }
}