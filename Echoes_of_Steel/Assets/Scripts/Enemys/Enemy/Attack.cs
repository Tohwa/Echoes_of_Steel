using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public EnemyBulletPool enemyBulletPool;
    public float fireRate = 1f;
    public float bulletDamage = 10f;
    private float nextFireTime;
    public Transform firePoint; // Der Punkt, von dem die Kugeln abgefeuert werden

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public NodeState Shoot()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            // Berechne die Richtung zum Spieler
            Vector3 direction = (player.position - firePoint.position).normalized;

            // Hole eine Kugel aus dem Pool und initialisiere sie
            GameObject bullet = enemyBulletPool.SpawnObject(firePoint.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Bullet>().Initialize(bulletDamage, direction);

            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }
}