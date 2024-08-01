using UnityEngine;

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
        if (tr != null) tr.Clear();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject;

        if (target.TryGetComponent(out Entity enemy))
        {
            enemy.Health -= weaponDamage;

            if (enemy.Health <= 0)
            {
                ApplyBreakEffect(target);
            }
        }
        else
        {
            ApplyBreakEffect(target);
        }

        Deactivate();
    }

    private void ApplyBreakEffect(GameObject target)
    {
        if (target.TryGetComponent(out MeshDestroy meshDestroy))
        {
            meshDestroy.DestroyMesh();
        }
    }
}