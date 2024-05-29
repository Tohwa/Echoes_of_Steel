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
    private float cameraMinDist = 2f;
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
    private float mouseX;
    private float mouseY;
    [SerializeField]
    private Vector2 scrollWheelValue;
    private float scrollValue;
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
        else if (!LMBstate && RMBstate)
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
        LMBstate = Input.GetKey(KeyCode.Mouse0);
        RMBstate = Input.GetKey(KeyCode.Mouse1);

        if (camState != CameraStates.cameraIdle)
        {
            if(camState == CameraStates.cameraRotate)
            {
                mouseX = Input.GetAxis("Mouse X") * cameraSpeed;
                currentPan += /*mouseAxis.x * cameraSpeed*/ mouseX;

                // steer interference = false
            }
            else if(camState == CameraStates.cameraSteer)
            {
                mouseX = Input.GetAxis("Mouse X") * cameraSpeed;
                currentPan += /*mouseAxis.x * cameraSpeed*/ mouseX;

                // steer interference = true
            }

            mouseY = Input.GetAxis("Mouse Y") * cameraSpeed;
            currentTilt -= /*mouseAxis.y * cameraSpeed*/ mouseY;
            currentTilt = Mathf.Clamp(currentTilt, -cameraMaxTilt, cameraMaxTilt);
        }

        scrollValue = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentDist -= /*scrollWheelValue.y*/ scrollValue;
        currentDist = Mathf.Clamp(currentDist, cameraMinDist, cameraMaxDist);
    }

    private void CameraTransformation()
    {
        switch (camState)
        {
            case CameraStates.cameraIdle:
                currentPan = player.transform.eulerAngles.y;
                break;
            case CameraStates.cameraRotate:

                break;
            case CameraStates.cameraSteer:

                break;
        }

        if(camState == CameraStates.cameraIdle)
        {
            currentTilt = 10f;
        }


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
        scrollWheelValue = ctx.ReadValue<Vector2>();
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
