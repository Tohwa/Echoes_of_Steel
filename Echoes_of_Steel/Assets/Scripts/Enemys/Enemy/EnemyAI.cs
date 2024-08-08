using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private BTNode rootNode;
    public PlayerDetection playerDetection;
    public Patrol patrol;
    public Attack attack;
    public BulletDetection bulletDetection;
    public Dash dash;

    private bool playerDetected = false;

    private void Start()
    {
        rootNode = CreateBehaviorTree();
    }

    private void Update()
    {
        NodeState state = rootNode.Evaluate();
        Debug.Log($"Root node evaluated with state: {state}");

        // Aktualisiere den playerDetected-Status
        playerDetected = playerDetection.detectionMeter >= playerDetection.detectionThreshold;

        // Steuere die Patrouille basierend auf playerDetected
        if (playerDetected)
        {
            patrol.StopPatrolling();
        }
        else
        {
            patrol.ResumePatrolling();
        }
    }

    private BTNode CreateBehaviorTree()
    {
        // Player detection
        ActionNode updateDetectionMeter = new ActionNode(playerDetection.UpdateDetectionMeter);
        ConditionNode playerDetectedCondition = new ConditionNode(() => playerDetection.detectionMeter >= playerDetection.detectionThreshold);

        // Attack
        ActionNode shoot = new ActionNode(attack.Shoot);

        // Evade
        ConditionNode bulletIncoming = new ConditionNode(() => bulletDetection.IsBulletIncoming());
        ActionNode dashAction = new ActionNode(() => dash.PerformDash());

        BTSequence dashSequence = new BTSequence(new List<BTNode> { bulletIncoming, dashAction });

        // Attack and Evade
        BTSelector attackSelector = new BTSelector(new List<BTNode> { dashSequence, shoot });
        BTSequence attackSequence = new BTSequence(new List<BTNode> { playerDetectedCondition, attackSelector });

        // Main: Attack if player is detected, else Patrol
        BTSelector mainSelector = new BTSelector(new List<BTNode> { attackSequence, moveToNextWaypoint });

        return mainSelector;
    }
}