using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class KunibertStateMachine : MonoBehaviour
{
    private AState currentState;
    [HideInInspector]

    public PatrolState patrolState;
    public CombatState combatState;

    public void ChangeState(AState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }

    private void Awake()
    {

    }

    void Start()
    {
        patrolState = new PatrolState(this);
        combatState = new CombatState(this);
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }
}