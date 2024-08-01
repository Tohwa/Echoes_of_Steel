using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 2f;
    public float rotationSpeed = 5f;

    public NodeState MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            return NodeState.FAILURE;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.y = 0; // Ignore vertical differences

        RotateInDirection(direction);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            return NodeState.SUCCESS;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        return NodeState.RUNNING;
    }

    public NodeState RoamRandomly()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        RotateInDirection(randomDirection);

        transform.position += randomDirection * speed * Time.deltaTime;
        return NodeState.RUNNING;
    }

    private void RotateInDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}