public class ActionNode : BTNode
{
    private System.Func<NodeState> action;

    public ActionNode(System.Func<NodeState> action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        return action();
    }
}