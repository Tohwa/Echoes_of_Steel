using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        GameObject.Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);        
    }
}
