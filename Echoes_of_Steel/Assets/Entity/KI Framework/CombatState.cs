using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatState : AState
{
    private KStateMachine pStateMachine;

    public CombatState(KStateMachine psm)
    {
        pStateMachine = psm;
    }

    public override void UpdateState()
    {
        CheckStateCondition();
    }

    private void CheckStateCondition()
    {
        if (true)
        {
            pStateMachine.ChangeState(pStateMachine.combatState);
        }
    }
}
