using UnityEngine;

public class BulletDetection : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask bulletLayer;

    public bool IsBulletIncoming()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, bulletLayer);
        return hits.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}