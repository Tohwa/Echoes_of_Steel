using System.Collections;
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

        //create a new object if none are available in the pool
        if (objectsInPool.Count <= 0)
        {
            GameObject newGO = Instantiate(prefab);
            currentObject = newGO.AddComponent<PooledObject>();
            currentObject.pool = this;
        }
        //grab oldest object of the list
        else
        {
            currentObject = objectsInPool[0];
            objectsInPool.Remove(currentObject);
        }

        //return object with new pos and rot data
        objectsInUse.Add(currentObject);

        currentObject.gameObject.SetActive(true);
        currentObject.gameObject.transform.position = position;
        currentObject.gameObject.transform.rotation = rotation;

        return currentObject.gameObject;
    }

    public void ReturnToPool(PooledObject objectToReturn)
    {
        //if object is already in pool return
        if (objectsInPool.Contains(objectToReturn))
        {
            return;
        }

        //if not in pool, disable it and return it to the pool
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