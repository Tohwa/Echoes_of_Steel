using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObjectPool bulletPool;
    public Transform bulletSpawnPoint;
    public Transform player;
    public float bulletDamage = 10f;

    public NodeState Shoot()
    {
        if (player == null) return NodeState.FAILURE;

        Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;
        GameObject bullet = bulletPool.SpawnObject(bulletSpawnPoint.position, Quaternion.LookRotation(direction));
        bullet.GetComponent<Bullet>().Initialize(bulletDamage, direction);

        return NodeState.SUCCESS;
    }
}