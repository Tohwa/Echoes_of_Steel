using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    private float weaponDamage;
    private Rigidbody rb;
    private TrailRenderer tr;

    private void OnEnable()
    {
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Invoke("Deactivate", lifeTime);
    }

    public void Initialize(float damage, Vector3 direction)
    {
        weaponDamage = damage;
        rb.velocity = direction * speed;
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
        tr.Clear();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject;
        //Debug.Log("Bullet collided with: " + target.name);

        if (target.TryGetComponent(out Entity enemy))
        {
            enemy.Health -= weaponDamage;
            //Debug.Log("Target has Health. Remaining Health: " + enemy.Health);

            if (enemy.Health <= 0)
            {
                //Debug.Log("Health is 0 or less. Applying BreakEffect.");
                ApplyBreakEffect(target);
            }
        }
        else
        {
            //Debug.Log("Target has no Health. Applying BreakEffect.");
            ApplyBreakEffect(target);
        }

        Deactivate();
    }

    private void ApplyBreakEffect(GameObject target)
    {
        if (target.TryGetComponent(out MeshDestroy meshDestroy))
        {
            //Debug.Log("MeshDestroy component found. Destroying mesh.");
            meshDestroy.DestroyMesh();
        }
        else
        {
            //Debug.LogWarning("No MeshDestroy component found on target.");
        }
    }
}