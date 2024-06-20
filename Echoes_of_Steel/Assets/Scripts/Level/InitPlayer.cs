using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;

    [SerializeField] private InteractionStatus interactionStatus;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject.Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        GameObject.Instantiate(cameraPrefab, playerSpawn.position, playerSpawn.rotation);

        Debug.Log("You have interacted with: " + interactionStatus.GetInteractionCount());
    }
}
