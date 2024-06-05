using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Fields

    [SerializeField] private float cameraPlayerHeight = 1.75f;
    [SerializeField] private float cameraChildHeight = 2f;
    [SerializeField] private float cameraChildOffset = 0.25f;
    [SerializeField] private float cameraMaxDist = 25f;
    [SerializeField] private float cameraMinDist = 2f;
    [SerializeField] private float cameraMaxTilt = 90f;
    [Range(0f, 4f)] public float cameraSpeed = 2f;
    [Range(0f, 4f)][SerializeField] private float zoomSpeed = 2f;
    [SerializeField] float currentPan;
    [SerializeField] private float currentTilt = 10f;
    [SerializeField] private float currentDist = 5f;

    private float panAngle;
    private float panOffSet;
    private bool camXAdjust;
    private bool camYAdjust;
    private float rotationXCushion = 3;
    private float rotationXSpeed = 0;
    private float rotationYSpeed = 0;
    private float yRotationMin = 0;
    private float yRotationMax = 20;
    [SerializeField] private float rotationDivider = 100;

    private PController player;
    public Transform tilt;
    private Camera mainCamera;

    [SerializeField] private bool LMBstate = false;
    [SerializeField] public bool RMBstate = false;
    [SerializeField] private Vector2 mouseAxis;
    private float mouseX;
    private float mouseY;
    [SerializeField] private Vector2 scrollWheelValue;
    private float scrollValue;

    public CameraStates camState = CameraStates.cameraIdle;
    public CameraCorrectState camCorrect = CameraCorrectState.OnlyWhileMoving;
    public CameraBasePosition basePosition = CameraBasePosition.PlayerPosition;

    [SerializeField] private bool collisionDebug;
    [SerializeField] private float collisionCushion = 0.35f;
    [SerializeField] private float adjustedDistance;
    [SerializeField] private LayerMask collisionMask;
    private Ray camRay;
    private RaycastHit hit;
    #endregion

    public enum CameraStates
    {
        cameraIdle,
        cameraRotate,
        cameraSteer
    }

    public enum CameraCorrectState
    {
        OnlyWhileMoving,
        OnlyHorizontalWhileMoving,
        AlwaysAdjust,
        NeverAdjust
    }

    public enum CameraBasePosition
    {
        PlayerPosition,
        ChildPosition
    }

    private void Awake()
    {
        transform.SetParent(null);
    }

    private void Start()
    {
        player = FindObjectOfType<PController>();
        mainCamera = Camera.main;

        if (basePosition == CameraBasePosition.PlayerPosition)
        {
            transform.position = player.transform.position + Vector3.up * cameraPlayerHeight;
            transform.rotation = player.transform.rotation;
        }
        else if (basePosition == CameraBasePosition.ChildPosition)
        {
            transform.position = new Vector3(player.transform.position.x + cameraChildOffset, player.transform.position.y, player.transform.position.z) + Vector3.up * cameraChildHeight;
            transform.rotation = player.transform.rotation;
        }

        tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        mainCamera.transform.position += tilt.forward * -currentDist;
    }

    private void Update()
    {
        if (!LMBstate && !RMBstate)
        {
            camState = CameraStates.cameraIdle;
        }
        else if (RMBstate)
        {
            camState = CameraStates.cameraRotate;
        }

        CheckCollision();
        CameraInput();
    }

    private void LateUpdate()
    {
        panAngle = Vector3.SignedAngle(transform.forward, player.transform.forward, Vector3.up);

        switch (camCorrect)
        {
            case CameraCorrectState.OnlyWhileMoving:
                if (player.isMoving)
                {
                    CameraXAdjust();
                    CameraYAdjust();
                }
                break;
            case CameraCorrectState.OnlyHorizontalWhileMoving:
                if (player.isMoving)
                {
                    CameraXAdjust();
                }
                break;
            case CameraCorrectState.AlwaysAdjust:
                CameraXAdjust();
                CameraYAdjust();
                break;
            case CameraCorrectState.NeverAdjust:
                CameraNeverAdjust();
                break;
        }

        CameraTransformation();
    }

    private void CheckCollision()
    {
        float camDistance = currentDist + collisionCushion;
        camRay.origin = transform.position;
        camRay.direction = -tilt.forward;

        if (Physics.Raycast(camRay, out hit, camDistance, collisionMask))
        {
            adjustedDistance = Vector3.Distance(camRay.origin, hit.point) - collisionCushion;
        }
        else
        {
            adjustedDistance = currentDist;
        }

        if (collisionDebug)
        {
            Debug.DrawLine(camRay.origin, camRay.origin + camRay.direction * camDistance, Color.black);
        }
    }

    private void CameraInput()
    {
        RMBstate = Input.GetKey(KeyCode.Mouse1);

        if (camState != CameraStates.cameraIdle)
        {
            if (camState == CameraStates.cameraRotate)
            {
                if (camCorrect != CameraCorrectState.NeverAdjust && !camXAdjust)
                {
                    camXAdjust = true;
                }

                if (camCorrect != CameraCorrectState.OnlyHorizontalWhileMoving && !camYAdjust)
                {
                    camYAdjust = true;
                }

                mouseX = Input.GetAxis("Mouse X") * cameraSpeed;
                currentPan += mouseX;
            }

            mouseY = Input.GetAxis("Mouse Y") * cameraSpeed;
            currentTilt -= mouseY;
            currentTilt = Mathf.Clamp(currentTilt, -cameraMaxTilt, cameraMaxTilt);
        }

        scrollValue = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentDist -= scrollValue;
        currentDist = Mathf.Clamp(currentDist, cameraMinDist, cameraMaxDist);
    }

    private void CameraNeverAdjust()
    {
        switch (camState)
        {
            case CameraStates.cameraIdle:
                currentPan = player.transform.eulerAngles.y - panOffSet;
                break;
            case CameraStates.cameraRotate:
                panOffSet = panAngle;
                break;
        }
    }

    private void CameraXAdjust()
    {
        if (camState != CameraStates.cameraRotate)
        {
            if (camXAdjust)
            {
                rotationXSpeed += Time.deltaTime;

                if (Mathf.Abs(panAngle) > rotationXCushion)
                {
                    currentPan = Mathf.Lerp(currentPan, currentPan + panAngle, rotationXSpeed / rotationDivider);
                }
                else
                {
                    camXAdjust = false;
                }
            }
            else
            {
                if (rotationXSpeed > 0)
                {
                    rotationXSpeed = 0;
                }

                currentPan = player.transform.eulerAngles.y;
            }
        }
    }

    private void CameraYAdjust()
    {
        if (camState != CameraStates.cameraRotate)
        {
            if (camYAdjust)
            {
                rotationYSpeed += Time.deltaTime;

                if (currentTilt >= yRotationMax || currentTilt <= yRotationMin)
                {
                    currentTilt = Mathf.Lerp(currentTilt, yRotationMax / 2, rotationYSpeed / rotationDivider);
                }
                else if (currentTilt < yRotationMax && currentTilt > yRotationMin)
                {
                    camYAdjust = false;
                }
            }
            else
            {
                if (rotationYSpeed > 0)
                {
                    rotationYSpeed = 0;
                }
            }
        }
    }

    private void CameraTransformation()
    {
        if (basePosition == CameraBasePosition.PlayerPosition)
        {
            transform.position = player.transform.position + Vector3.up * cameraPlayerHeight;
        }
        else if (basePosition == CameraBasePosition.ChildPosition)
        {
            transform.position = new Vector3(player.transform.position.x + cameraChildOffset, player.transform.position.y, player.transform.position.z) + Vector3.up * cameraChildHeight;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(currentTilt, tilt.eulerAngles.y, tilt.eulerAngles.z);
        mainCamera.transform.position = transform.position + tilt.forward * -adjustedDistance;
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