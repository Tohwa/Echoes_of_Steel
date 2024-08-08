using UnityEngine;

public class BulletDetection : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask bulletLayer;

    public bool IsBulletIncoming()
    {
        Debug.Log("IsBulletIncoming called");
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, bulletLayer);
        Debug.Log($"Detected {hits.Length} bullets in IsBulletIncoming.");
        return hits.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}