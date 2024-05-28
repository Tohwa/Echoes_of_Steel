using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    #region Fields

    //Camera Variables
    public float cameraHeight = 1.75f, cameraMaxDist = 25f;
    float cameraMaxTilt = 90f;
    [Range(0f, 4f)]
    public float cameraSpeed = 2f;
    float currentPan, currentTilt = 10f, currentDist = 5f;

    //References
    //playerController = player;
    public Transform tilt;
    Camera mainCamera;

    #endregion
    void Start()
    {
        /*
         player = FindObjectOfType<PlayerController>();
         */
        mainCamera = Camera.main;

        // transform.position = player.transform.position + Vector3.up * cameraHeight
    }

    void Update()
    {
        
    }
}
