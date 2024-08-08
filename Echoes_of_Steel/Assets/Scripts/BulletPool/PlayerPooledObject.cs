using UnityEngine;

public class EnemyPooledObject : MonoBehaviour
{
    public EnemyBulletPool pool;
    private void OnDisable()
    {
        pool.ReturnToPool(this);
    }

    private void OnDestroy()
    {
        pool.RemoveObject(this);
    }
}
