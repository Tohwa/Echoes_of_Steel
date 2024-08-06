using System.Collections.Generic;

public class BTSequence : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public BTSequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool anyNodeRunning = false;

        foreach (BTNode node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    return NodeState.FAILURE;
                case NodeState.RUNNING:
                    anyNodeRunning = true;
                    break;
            }
        }
        return anyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
    }
}