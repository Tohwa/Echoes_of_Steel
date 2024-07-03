using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayer : MonoBehaviour
{
    #region Variables

    [Header("Transform Variables")]
    private Transform playerSpawn;

    [Header("GameObject Variables")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;

    [Header("Interaction Variables")]
    [SerializeField] private InteractionStatus interactionStatus;

    #endregion

    void Start()
    {
        if(playerSpawn == null) 
        {
            playerSpawn = GameObject.Find("PlayerSpawn").transform;    
        }
        else
        {
            Debug.LogError("Player Spawn could not be found!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject.Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        GameObject.Instantiate(cameraPrefab, playerSpawn.position, playerSpawn.rotation);

        Debug.Log("You have interacted with: " + interactionStatus.GetInteractionCount());
    }
}
