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
            Vector3 dashDirection = GetDashDirection();
            Vector3 dashTarget = transform.position + dashDirection * dashDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(dashTarget, out hit, dashDistance, 1))
            {
                agent.Warp(hit.position);
                lastDashTime = Time.time;
                Debug.Log("Dash successful.");
                return NodeState.SUCCESS;
            }
            Debug.Log("Dash failed: No valid target position.");
        }
        else
        {
            Debug.Log("Dash on cooldown.");
        }
        return NodeState.FAILURE;
    }

    private Vector3 GetDashDirection()
    {
        // Dash zur Seite (90°-Winkel nach rechts)
        return Quaternion.Euler(0, 90, 0) * transform.forward;
    }
}