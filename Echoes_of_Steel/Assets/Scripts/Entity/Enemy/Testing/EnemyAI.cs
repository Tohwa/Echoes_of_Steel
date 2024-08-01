using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private BTNode rootNode;

    public Patrol patrol;
    public PlayerDetection playerDetection;
    public Attack attack;
    public Dash dash;
    public BulletDetection bulletDetection;
    private Transform player;

    public float rotationSpeed = 5f;

    void Start()
    {
        // Erstelle den Behavior Tree
        rootNode = CreateBehaviorTree();
        player = playerDetection.player.transform;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        rootNode.Evaluate();

        if (playerDetection.detectionMeter >= playerDetection.detectionThreshold)
        {
            LookAtPlayer();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Ignore vertical differences

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private BTNode CreateBehaviorTree()
    {
        // Patrouillieren und Roamen
        ActionNode moveToNextWaypoint = new ActionNode(patrol.MoveToNextWaypoint);
        ActionNode roamRandomly = new ActionNode(patrol.RoamRandomly);
        BTSequence patrolSequence = new BTSequence(new List<BTNode> { moveToNextWaypoint, roamRandomly });

        // Spielerdetektion
        ActionNode updateDetectionMeter = new ActionNode(playerDetection.UpdateDetectionMeter);
        ConditionNode playerDetected = new ConditionNode(() => playerDetection.detectionMeter >= playerDetection.detectionThreshold);

        // Angriff
        ActionNode shoot = new ActionNode(attack.Shoot);

        // Ausweichen
        ConditionNode bulletIncoming = new ConditionNode(bulletDetection.IsBulletIncoming);
        ActionNode dashAction = new ActionNode(dash.PerformDash);

        BTSequence dashSequence = new BTSequence(new List<BTNode> { bulletIncoming, dashAction });

        // Zusammensetzen des Behavior Trees
        BTSelector attackSelector = new BTSelector(new List<BTNode> { dashSequence, shoot });
        BTSequence attackSequence = new BTSequence(new List<BTNode> { playerDetected, attackSelector });
        BTSelector mainSelector = new BTSelector(new List<BTNode> { attackSequence, patrolSequence });

        return mainSelector;
    }
}