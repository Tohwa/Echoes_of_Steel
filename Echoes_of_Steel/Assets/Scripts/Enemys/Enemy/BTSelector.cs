using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    private List<BTNode> nodes;

    public BTSelector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Evaluating BTSelector Node...");
        foreach (BTNode node in nodes)
        {
            NodeState nodeState = node.Evaluate();
            Debug.Log($"BTSelector Node {node.GetType().Name} evaluated with state: {nodeState}");
            if (nodeState == NodeState.SUCCESS)
            {
                return NodeState.SUCCESS;
            }
            if (nodeState == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
        }
        return NodeState.FAILURE;
    }
}