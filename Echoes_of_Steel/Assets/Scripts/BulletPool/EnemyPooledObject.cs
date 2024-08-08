using UnityEngine;

public class PlayerPooledObject : MonoBehaviour
{
    public PlayerBulletPool pool;
    private void OnDisable()
    {
        pool.ReturnToPool(this);
    }

    private void OnDestroy()
    {
        pool.RemoveObject(this);
    }
}