using UnityEngine;
using UnityEngine.InputSystem;

public class PatrolState : AState
{
    private KunibertStateMachine kStateMachine;

    public PatrolState(KunibertStateMachine ksm)
    {
        kStateMachine = ksm;
    }

    public override void UpdateState()
    {
        CheckStateCondition();
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
        Debug.Log("Entered Patrol State");
    }

    public override void ExitState()
    {
        Debug.Log("Exited Patrol State");
    }
}
