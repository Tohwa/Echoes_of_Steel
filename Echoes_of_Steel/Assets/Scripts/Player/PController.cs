using Adobe.Substance;
using System;
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
    public InputAction aim;
    public InputAction shoot;
    public InputAction shield;
    public InputAction hover;
    public InputAction interact;
    public InputAction journal;
    public InputAction pause;

    [Header("Framework Variables")]
    public Camera mainCamera;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    public LayerMask Ground;
    private Vector2 moveInput;
    public bool isMoving;
    public bool isGrounded;

    [Header("Move Variables")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    private Vector3 currentVelocity;

    [Header("Jump Variables")]
    public float jumpForce = 5f;
    [SerializeField] private bool canDoubleJump;
    private bool isJumping;
    public float jumpCooldown = 0.1f;
    private float lastJumpTime;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    private float lastGroundedTime;
    private float jumpBufferCounter;
    public float manualGravity = -20f;
    private bool applyManualGravity;

    [Header("Dash Variables")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float dashTime;
    private bool isDashing;
    private float lastDashTime;

    [Header("Weapon Variables")]
    public float weaponDamage;
    public float fireCooldown = 0.5f;
    private float lastFireTime;
    public bool automatic;
    public GameObjectPool bulletPool;
    public Transform bulletSpawn;
    private Transform pCamera;

    [Header("Hover Variables")]
    public float hoverFallSpeed = 2f;
    private bool isHovering;

    [Header("Interactable Variables")]
    private IInteractable interactable;

    [Header("PauseMenuHandler Variable")]
    public PauseMenuHandler pauseMenuHandler;

    #endregion

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        pCamera = Camera.main.transform;
        pauseMenuHandler = FindObjectOfType<PauseMenuHandler>();
    }
    private void Update()
    {
        GroundCheck();
        isMoving = moveInput != Vector2.zero;

        HandleJumpBuffering();
        if (!GameManager.Instance.gamePaused)
        {

            WeaponHandler();
        }

        if (isHovering)
        {
            Hover();
        }
    }

    private void FixedUpdate()
    {
        if (!DialogueManager.isActive && !GameManager.Instance.gamePaused)
        {
            if (isDashing)
            {
                Dash();
            }
            else
            {
                Move();
            }

            if (isJumping)
            {
                PerformJump();
            }

            // Manuelle Schwerkraft anwenden
            if (applyManualGravity && rb.velocity.y <= 0)
            {
                rb.AddForce(Vector3.up * manualGravity, ForceMode.Acceleration);
            }
        }
    }

    void OnEnable()
    {
        movement.Enable();
        jump.Enable();
        dash.Enable();
        aim.Enable();
        shoot.Enable();
        shield.Enable();
        hover.Enable();
        interact.Enable();
        journal.Enable();
        pause.Enable();

        jump.performed += OnJumpInput;
        movement.performed += OnMovementInput;
        movement.canceled += OnMovementInput;
        dash.performed += OnDashInput;
        hover.performed += OnHoverHold;
        hover.canceled += OnHoverRelease;

        aim.performed += OnAimHold;
        aim.canceled += OnAimRelease;
        shoot.performed += OnShootInput;
        shoot.canceled += OnShootCancel;
        shield.performed += OnShieldHold;
        shield.canceled += OnShieldRelease;

        interact.performed += OnInteractInput;
        interact.canceled += OnInteractCancel;
        journal.performed += OnJournalInput;
        pause.performed += OnPauseInput;
    }
    void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        dash.Disable();
        aim.Disable();
        shoot.Disable();
        shield.Disable();
        hover.Disable();
        interact.Disable();
        journal.Disable();
        pause.Disable();

        movement.performed -= OnMovementInput;
        movement.canceled -= OnMovementInput;
        jump.performed -= OnJumpInput;
        dash.performed -= OnDashInput;

        aim.performed -= OnAimHold;
        aim.canceled -= OnAimRelease;
        shoot.performed -= OnShootInput;
        shoot.canceled -= OnShootCancel;
        shield.performed -= OnShieldHold;
        shield.canceled -= OnShieldRelease;

        hover.performed -= OnHoverHold;
        hover.canceled -= OnHoverRelease;
        interact.performed -= OnInteractInput;
        interact.canceled -= OnInteractCancel;
        journal.performed -= OnJournalInput;
        pause.performed -= OnPauseInput;
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

        Vector3 targetVelocity = (forward * moveInput.y + right * moveInput.x) * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            animator.SetBool("IsWalking", false);
        }

        if (currentVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (isGrounded || (canDoubleJump && !isGrounded))
        {
            isJumping = true;
        }
    }
    private void HandleJumpBuffering()
    {
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            canDoubleJump = true; // Reset double jump ability when grounded
        }

        if (jump.triggered)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump buffering logic
        if (jumpBufferCounter > 0)
        {
            if (isGrounded || (Time.time - lastGroundedTime <= coyoteTime))
            {
                isJumping = true;
                jumpBufferCounter = 0; // Reset jump buffer counter
            }
        }
    }
    private void PerformJump()
    {
        if (isGrounded || canDoubleJump)
        {
            if (!isGrounded && canDoubleJump)
            {
                canDoubleJump = false; // Use up double jump
            }

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset vertical velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
            isJumping = false; // Reset jumping flag after the jump is performed
            applyManualGravity = true; // Start applying manual gravity after the jump
            animator.SetBool("IsJumping", true);
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

    private void OnAimHold(InputAction.CallbackContext context)
    {
        animator.SetBool("IsAiming", true);
    }

    private void OnAimRelease(InputAction.CallbackContext context)
    {
        animator.SetBool("IsAiming", false);
    }

    private void OnShieldHold(InputAction.CallbackContext context)
    {
        animator.SetBool("IsShielding", true);
    }
    private void OnShieldRelease(InputAction.CallbackContext context)
    {
        animator.SetBool("IsShielding", false);

    }

    private void OnShootInput(InputAction.CallbackContext context)
    {
        animator.SetBool("IsShooting", true);
    }
    private void OnShootCancel(InputAction.CallbackContext context)
    {
        animator.SetBool("IsShooting", false);

    }



    private void OnHoverHold(InputAction.CallbackContext context)
    {
        Debug.Log("Hovering started");
        isHovering = true;
    }
    private void OnHoverRelease(InputAction.CallbackContext context)
    {
        Debug.Log("Hovering stopped");
        isHovering = false;
    }
    private void Hover()
    {
        if (!isGrounded)
        {
            Vector3 hoverVelocity = rb.velocity;
            hoverVelocity.y = Mathf.Max(hoverVelocity.y, -hoverFallSpeed);
            rb.velocity = hoverVelocity;
        }
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {

        if (interactable != null && !DialogueManager.isActive)
        {
            

            animator.SetBool("IsInteracting", true);
            
            interactable.Interact();
            interactable = null;
            
            Debug.Log("Interacted with object");
        }

    }

    private void OnInteractCancel(InputAction.CallbackContext context)
    {
        animator.SetBool("IsInteracting", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("Collided");
            interactable = other.gameObject.GetComponent<IInteractable>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            interactable = null;
        }
    }

    private void OnJournalInput(InputAction.CallbackContext context)
    {
        DialogueManager.instance.OpenJournal();
    }
    private void OnPauseInput(InputAction.CallbackContext context)
    {
        pauseMenuHandler.PauseGame();
    }

    private void WeaponHandler()
    {
        if (!DialogueManager.isActive)
        {
            if (automatic)
            {
                if (shoot.ReadValue<float>() > 0.1f && Time.time >= lastFireTime + fireCooldown)
                {
                    lastFireTime = Time.time;
                    Shoot();
                }
            }
            else if (shoot.triggered && Time.time >= lastFireTime + fireCooldown)
            {
                lastFireTime = Time.time;
                Shoot();
            }
        }
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
        float rayLength = 0.2f;
        Vector3 rayOrigin = capsuleCollider.bounds.center;
        rayOrigin.y = capsuleCollider.bounds.min.y + 0.1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, rayLength, Ground))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            applyManualGravity = false; // Stop applying manual gravity when grounded
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