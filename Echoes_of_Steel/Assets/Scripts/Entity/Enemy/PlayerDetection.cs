using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float detectionThreshold = 5f;
    public float detectionMeter = 0f;
    public float detectionRate = 1f;

    void Update()
    {
        UpdateDetectionMeter();
    }

    public NodeState UpdateDetectionMeter()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            detectionMeter += detectionRate * Time.deltaTime;
        }
        else
        {
            detectionMeter = Mathf.Max(0, detectionMeter - detectionRate * Time.deltaTime);
        }

        return detectionMeter >= detectionThreshold ? NodeState.SUCCESS : NodeState.RUNNING;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (detectionMeter >= detectionThreshold)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}