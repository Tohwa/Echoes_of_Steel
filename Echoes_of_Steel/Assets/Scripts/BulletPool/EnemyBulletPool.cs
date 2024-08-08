using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletPools/Enemy Bullet Pool")]
public class EnemyBulletPool : ScriptableObject
{
    public GameObject prefab;

    private List<EnemyPooledObject> objectsInPool = new List<EnemyPooledObject>();
    private List<EnemyPooledObject> objectsInUse = new List<EnemyPooledObject>();

    public GameObject SpawnObject(Vector3 position, Quaternion rotation)
    {
        EnemyPooledObject currentObject;

        // Create a new object if none are available in the pool
        if (objectsInPool.Count <= 0)
        {
            GameObject newGO = Instantiate(prefab);
            currentObject = newGO.AddComponent<EnemyPooledObject>();
            currentObject.pool = this;
        }
        else
        {
            // Grab the oldest object in the pool
            currentObject = objectsInPool[0];
            objectsInPool.RemoveAt(0);
        }

        objectsInUse.Add(currentObject);

        // Set the position and rotation of the object
        currentObject.transform.position = position;
        currentObject.transform.rotation = rotation;
        currentObject.gameObject.SetActive(true);

        return currentObject.gameObject;
    }

    public void ReturnToPool(EnemyPooledObject objectToReturn)
    {
        // If the object is already in the pool, return
        if (objectsInPool.Contains(objectToReturn))
        {
            return;
        }

        // If not in the pool, disable it and return it to the pool
        objectToReturn.gameObject.SetActive(false);
        objectsInUse.Remove(objectToReturn);
        objectsInPool.Add(objectToReturn);
    }

    public void RemoveObject(EnemyPooledObject objectToRemove)
    {
        objectsInPool.Remove(objectToRemove);
        objectsInUse.Remove(objectToRemove);
    }
}
