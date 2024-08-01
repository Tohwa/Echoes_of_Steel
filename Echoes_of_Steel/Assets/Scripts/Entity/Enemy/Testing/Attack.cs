using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform firePoint;
    public BulletPool bulletPool;

    public NodeState Shoot()
    {
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * 10f; // Beispielgeschwindigkeit

        return NodeState.SUCCESS;
    }
}