using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float roamRadius = 5f;
    public float waypointTolerance = 0.5f;
    public float roamTime = 2f; // Time spent roaming at a waypoint
    private float roamTimer = 0f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    public NodeState MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            return NodeState.FAILURE;
        }

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            // Roam for a while after reaching the waypoint
            if (roamTimer <= 0)
            {
                roamTimer = roamTime;
                RoamRandomly();
            }
            else
            {
                roamTimer -= Time.deltaTime;
                if (roamTimer <= 0)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    public void RoamRandomly()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1))
        {
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
    }
}