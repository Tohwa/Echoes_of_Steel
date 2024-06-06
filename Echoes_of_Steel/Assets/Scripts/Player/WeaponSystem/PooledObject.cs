using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public GameObjectPool pool;

    private void OnDisable()
    {
        pool.ReturnToPool(this);
    }

    private void OnDestroy()
    {
        pool.RemoveObject(this);
    }
}
