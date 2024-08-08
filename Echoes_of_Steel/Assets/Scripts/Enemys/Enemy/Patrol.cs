using UnityEngine.AI;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    public NodeState MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return NodeState.FAILURE;

        if (!agent.isStopped) // Nur fortfahren, wenn der Agent nicht gestoppt ist
        {
            agent.destination = waypoints[currentWaypointIndex].position;

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.destination = waypoints[currentWaypointIndex].position;
            }

            return NodeState.RUNNING;
        }

        return NodeState.FAILURE;
    }

    public void StopPatrolling()
    {
        agent.isStopped = true;
    }

    public void ResumePatrolling()
    {
        agent.isStopped = false;
        MoveToNextWaypoint();
    }
}