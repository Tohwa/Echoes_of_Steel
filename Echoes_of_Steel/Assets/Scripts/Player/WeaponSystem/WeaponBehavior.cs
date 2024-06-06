using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public float fireCooldown;
    private float curCooldown;

    public bool automatic;
    public GameObjectPool bulletPool;

    public float weaponDamage;
    public Transform bulletSpawn;

    private Transform pCamera;

    void Start()
    {
        curCooldown = fireCooldown;
        pCamera = Camera.main.transform;
    }

    void Update()
    {
        if (automatic)
        {
            if (Input.GetMouseButton(0) && curCooldown <= 0f)
            {
                Shoot();
                curCooldown = fireCooldown;
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && curCooldown <= 0f)
            {
                Shoot();
                curCooldown = fireCooldown;
            }
        }
        curCooldown -= Time.deltaTime;
    }

    void Shoot()
    {
        Vector3 spawnPosition = bulletSpawn.position;
        Quaternion spawnRotation = bulletSpawn.rotation;
        GameObject bullet = bulletPool.SpawnObject(spawnPosition, spawnRotation);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Initialize(weaponDamage, bulletSpawn.forward);
    }

    private void OnDrawGizmos()
    {
        if (pCamera == null)
        {
            pCamera = Camera.main.transform;
        }

        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = pCamera.position;
        Vector3 rayDirection = pCamera.forward;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection);

        if (Physics.Raycast(rayOrigin, pCamera.forward, out RaycastHit hitInfo))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitInfo.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(rayOrigin + rayDirection, 0.1f);
        }
    }
}