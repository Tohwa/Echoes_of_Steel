using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Drown : MonoBehaviour
{
    [SerializeField] private GameObject spawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = spawn.transform.position;
        }
    }
}
