using UnityEngine;
using BehaviorTree;

public class TaskAttack : Node
{
    //private Animator _animator;

    private Transform _transform;
    private Transform _lastTarget;
    private EnemyManager _enemyManager;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public TaskAttack(Transform transform)
    {
        _transform = transform;
        //_animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<EnemyManager>();
            _lastTarget = target;
        }

        // Check if the target is out of attack range
        if (Vector3.Distance(_transform.position, target.position) > GuardBT.attackRange)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            bool enemyIsDead = _enemyManager.TakeHit();
            if (enemyIsDead)
            {
                ClearData("target");
                //_animator.SetBool("Attacking", false);
                //_animator.SetBool("Walking", true);
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
