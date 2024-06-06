using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PController : MonoBehaviour
{
    public InputAction movement;
    public InputAction jump;
    public InputAction dash;
    public LayerMask Ground;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float maxJumps = 2f;
    public float curJumps;
    public bool isGrounded;

    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Camera mainCamera;

    public float dashSpeed = 20f; // Speed during dash
    public float dashDuration = 0.2f; // Duration of dash
    public float dashCooldown = 1f; // Cooldown between dashes
    private float dashTime;
    private bool isDashing;
    private float lastDashTime;
    public bool isMoving;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        GroundCheck();
        isMoving = moveInput != Vector2.zero;
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
        dash.Enable(); // Enable dash input

        jump.performed += OnJumpInput;
        movement.performed += OnMovementInput;
        movement.canceled += OnMovementInput;
        dash.performed += OnDashInput; // Add dash input handler
    }

    void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        dash.Disable(); // Disable dash input

        movement.performed -= OnMovementInput;
        movement.canceled -= OnMovementInput;
        jump.performed -= OnJumpInput;
        dash.performed -= OnDashInput; // Remove dash input handler
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
    }
}