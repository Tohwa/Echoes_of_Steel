using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatState : AState
{
    private KunibertStateMachine kStateMachine;

    public CombatState(KunibertStateMachine ksm)
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
}
