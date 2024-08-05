using UnityEngine;
using UnityEngine.AI;

public class Dash : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashCooldown = 3f;
    private float lastDashTime = -10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public NodeState PerformDash()
    {
        if (Time.time > lastDashTime + dashCooldown)
        {
            Vector3 dashDirection = transform.right * dashDistance; // Dash zur Seite
            Vector3 dashTarget = transform.position + dashDirection;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(dashTarget, out hit, dashDistance, 1))
            {
                agent.Warp(hit.position);
                lastDashTime = Time.time;
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}