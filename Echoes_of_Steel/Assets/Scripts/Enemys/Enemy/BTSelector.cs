using System.Collections.Generic;

public class BTSelector : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public BTSelector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (BTNode node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.SUCCESS:
                    return NodeState.SUCCESS;
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
            }
        }
        return NodeState.FAILURE;
    }
}