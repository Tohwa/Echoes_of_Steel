using UnityEngine;
using UnityEngine.InputSystem;

public class PatrolState : AState
{
    private KStateMachine kStateMachine;

    public PatrolState(KStateMachine ksm)
    {
        kStateMachine = ksm;
    }

    public override void UpdateState()
    {
        CheckStateCondition();
        //OnMove();
    }

    private void CheckStateCondition()
    {
        if (true)
        {
            kStateMachine.ChangeState(kStateMachine.combatState);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Entered Exploration State");
    }

    public override void ExitState()
    {
        Debug.Log("Exited Exploration State");
    }
}
