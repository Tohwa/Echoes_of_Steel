using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/GameObject Pool")]
public class GameObjectPool : ScriptableObject
{
    public GameObject prefab;

    private List<PooledObject> objectsInPool = new List<PooledObject>();
    private List<PooledObject> objectsInUse = new List<PooledObject>();

    public GameObject SpawnObject(Vector3 position, Quaternion rotation)
    {
        PooledObject currentObject;

        // Create a new object if none are available in the pool
        if (objectsInPool.Count <= 0)
        {
            GameObject newGO = Instantiate(prefab);
            currentObject = newGO.AddComponent<PooledObject>();
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

    public void ReturnToPool(PooledObject objectToReturn)
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

    public void RemoveObject(PooledObject objectToRemove)
    {
        objectsInPool.Remove(objectToRemove);
        objectsInUse.Remove(objectToRemove);
    }
}
