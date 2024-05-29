using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Fields

    //Camera Variables
    [SerializeField]
    private float cameraHeight = 1.75f;
    [SerializeField]
    private float cameraMaxDist = 25f;
    [SerializeField]
    private float cameraMaxTilt = 90f;
    [Range(0f, 4f)][SerializeField]
    private float cameraSpeed = 2f;
    [Range(0f, 4f)][SerializeField]
    private float zoomSpeed = 2f;
    [SerializeField]
    float currentPan;
    [SerializeField]
    private float currentTilt = 10f;
    [SerializeField]
    private float currentDist = 5f;

    //References
    PlayerController player;
    public Transform tilt;
    Camera mainCamera;

    //input Variables
    [SerializeField]
    private bool LMBstate = false;
    [SerializeField]
    private bool RMBstate = false;
    [SerializeField]
    private Vector2 mouseAxis;
    [SerializeField]
    private Vector2 scroolWheelValue;

    //CameraState
    public CameraStates camState = CameraStates.cameraIdle;

    #endregion

    public enum CameraStates
    {
        cameraIdle,
        cameraRotate,
        cameraSteer
    }

    private void Start()
    {
        
        player = FindObjectOfType<PlayerController>();
        
        mainCamera = Camera.main;

        transform.position = player.transform.position + Vector3.up * cameraHeight;
        transform.rotation = player.transform.rotation;

        tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        mainCamera.transform.position += tilt.forward * -currentDist;
    }

    private void Update()
    {
        if (!LMBstate && !RMBstate)
        {
            camState = CameraStates.cameraIdle;
        }
        else if(LMBstate && !RMBstate)
        {
            camState = CameraStates.cameraRotate;
        }
        else if (RMBstate && !LMBstate)
        {
            camState = CameraStates.cameraSteer;
        }

        CameraInput();
    }

    private void LateUpdate()
    {
        CameraTransformation();
    }

    private void CameraInput()
    {
        if(camState != CameraStates.cameraIdle)
        {
            if(camState == CameraStates.cameraRotate)
            {
                currentPan += mouseAxis.x * cameraSpeed;
            }

            currentTilt -= mouseAxis.y * cameraSpeed;
            currentTilt = Mathf.Clamp(currentTilt, -cameraMaxTilt, cameraMaxTilt);
        }

        currentDist -= scroolWheelValue.y;
        currentDist = Mathf.Clamp(currentDist, 0, cameraMaxDist);
    }

    private void CameraTransformation()
    {
        switch (camState)
        {
            case CameraStates.cameraIdle:
                currentPan = player.transform.eulerAngles.y;
                currentTilt = 10f;
                break;
            case CameraStates.cameraRotate:

                break;
            case CameraStates.cameraSteer:

                break;
        }

        currentPan = player.transform.eulerAngles.y;

        transform.position = player.transform.position + Vector3.up * cameraHeight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(currentTilt, tilt.eulerAngles.y, tilt.eulerAngles.z);
        mainCamera.transform.position = transform.position + tilt.forward * -currentDist;
    }

    public void GetMouseAxis(InputAction.CallbackContext ctx)
    {
        mouseAxis = ctx.ReadValue<Vector2>();
    }

    public void GetScrollWheelValue(InputAction.CallbackContext ctx)
    {
        scroolWheelValue = ctx.ReadValue<Vector2>();
    }

    public void GetLMBState(InputAction.CallbackContext ctx)
    {
        LMBstate = ctx.performed;
    }

    public void GetRMBState(InputAction.CallbackContext ctx)
    {
        RMBstate = ctx.performed;
    }
}
