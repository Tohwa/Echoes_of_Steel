using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionStatus", menuName = "ScriptableObjects/InteractionStatus", order = 1)]
public class InteractionStatus : ScriptableObject
{
    private HashSet<string> interactedObjects = new HashSet<string>();

    public void AddInteractedObject(string objectName)
    {
        if (!interactedObjects.Contains(objectName))
        {
            interactedObjects.Add(objectName);
        }
    }

    public bool HasInteracted(string objectName)
    {
        return interactedObjects.Contains(objectName);
    }
}
