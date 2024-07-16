using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Drown : MonoBehaviour
{
    #region Variables

    [Header("Transform Variables")]
    private Transform playerSpawn;

    #endregion

    private void Start()
    {
        if (playerSpawn == null)
        {
            playerSpawn = GameObject.Find("PlayerSpawn").transform;
        }
        else
        {
            Debug.LogError("Player Spawn could not be found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = playerSpawn.transform.position;
        }
    }
}
