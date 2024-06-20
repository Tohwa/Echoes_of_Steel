using UnityEngine;
using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        // Check if the target is out of FOV range
        if (Vector3.Distance(_transform.position, target.position) > GuardBT.fovRange)
        {
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            Vector3 direction = (target.position - _transform.position).normalized;
            direction.y = 0; // Prevent rotation on the X axis

            _transform.position = Vector3.MoveTowards(
                _transform.position, target.position, GuardBT.speed * Time.deltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * GuardBT.speed);
        }

        state = NodeState.RUNNING;
        return state;
    }
}