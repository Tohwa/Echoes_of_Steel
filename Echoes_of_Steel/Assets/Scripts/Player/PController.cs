using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PController : MonoBehaviour
{
    #region Variables
    [Header("Actionmaps")]
    public InputAction movement;
    public InputAction jump;
    public InputAction dash;
    public InputAction shooting;

    [Header("Framework Variables")]
    public Camera mainCamera;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    public LayerMask Ground;
    private Vector2 moveInput;
    public bool isMoving;
    public bool isGrounded;

    [Header("Simple Move and Jump Variables")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float maxJumps = 2f;
    public float curJumps;

    [Header("Dash Variables")]
    public float dashSpeed = 20f; // Speed during dash
    public float dashDuration = 0.2f; // Duration of dash
    public float dashCooldown = 1f; // Cooldown between dashes
    private float dashTime;
    private bool isDashing;
    private float lastDashTime;

    [Header("Weapon Variables")]
    public float fireCooldown = 0.5f;
    private float lastFireTime;
    public bool automatic;
    public GameObjectPool bulletPool;
    public float weaponDamage;
    public Transform bulletSpawn;
    private Transform pCamera;
    #endregion

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        pCamera = Camera.main.transform;
    }

    private void Update()
    {
        GroundCheck();
        isMoving = moveInput != Vector2.zero;

        // Handle shooting
        if (automatic)
        {
            if (shooting.ReadValue<float>() > 0.1f && Time.time >= lastFireTime + fireCooldown)
            {
                lastFireTime = Time.time;
                Shoot();
            }
        }
        else if (shooting.triggered && Time.time >= lastFireTime + fireCooldown)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Dash();
        }
        else
        {
            Move();
        }
    }

    void OnEnable()
    {
        movement.Enable();
        jump.Enable();
        dash.Enable();
        shooting.Enable();

        jump.performed += OnJumpInput;
        movement.performed += OnMovementInput;
        movement.canceled += OnMovementInput;
        dash.performed += OnDashInput;
        shooting.performed += OnShootingInput;
    }

    void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        dash.Disable();
        shooting.Disable();

        movement.performed -= OnMovementInput;
        movement.canceled -= OnMovementInput;
        jump.performed -= OnJumpInput;
        dash.performed -= OnDashInput;
        shooting.performed -= OnShootingInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void Move()
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movementVector = forward * moveInput.y + right * moveInput.x;
        movementVector *= moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movementVector);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        Jump();
    }
    private void Jump()
    {
        if (isGrounded || curJumps > 0)
        {
            curJumps--;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnDashInput(InputAction.CallbackContext context)
    {
        if (Time.time >= lastDashTime + dashCooldown)
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
            lastDashTime = Time.time;
        }
    }
    private void Dash()
    {
        if (Time.time < dashTime)
        {
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 dashDirection = forward * moveInput.y + right * moveInput.x;
            dashDirection.Normalize();

            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isDashing = false;
        }
    }

    private void OnShootingInput(InputAction.CallbackContext context)
    {
        
    }
    private void Shoot()
    {
        Vector3 spawnPosition = bulletSpawn.position;
        Quaternion spawnRotation = bulletSpawn.rotation;
        GameObject bullet = bulletPool.SpawnObject(spawnPosition, spawnRotation);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Initialize(weaponDamage, bulletSpawn.forward);
    }

    public bool GroundCheck()
    {
        float rayLength = 0.2f; // Adjust the length of the ray as needed
        Vector3 rayOrigin = capsuleCollider.bounds.center;
        rayOrigin.y = capsuleCollider.bounds.min.y + 0.1f; // Start the ray from just above the bottom of the collider

        if (Physics.Raycast(rayOrigin, Vector3.down, rayLength, Ground))
        {
            isGrounded = true;
            curJumps = maxJumps;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        // Draw the CapsuleCollider bounds
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(capsuleCollider.bounds.center, capsuleCollider.bounds.size);

        // Draw the GroundCheck ray
        Gizmos.color = Color.red;
        Vector3 rayOrigin = capsuleCollider.bounds.center;
        rayOrigin.y = capsuleCollider.bounds.min.y + 0.1f; // Start the ray from just above the bottom of the collider
        float rayLength = 0.2f;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayLength);

        if (pCamera == null)
        {
            pCamera = Camera.main.transform;
        }

        Gizmos.color = Color.yellow;
        Vector3 cameraRayOrigin = pCamera.position;
        Vector3 cameraRayDirection = pCamera.forward;
        Gizmos.DrawLine(cameraRayOrigin, cameraRayOrigin + cameraRayDirection * 10);

        if (Physics.Raycast(cameraRayOrigin, pCamera.forward, out RaycastHit hitInfo))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitInfo.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(cameraRayOrigin + cameraRayDirection * 10, 0.1f);
        }
    }
}