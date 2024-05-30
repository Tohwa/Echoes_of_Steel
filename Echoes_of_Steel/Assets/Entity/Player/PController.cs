using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;
    private Vector3 moveDirection;
    private Vector2 inputVector;
    private bool jump;
    private float gravity = -9.81f;
    private float verticalVelocity;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        Move();
        GroundCheck();
        
    }

    private void GroundCheck()
    {
        Physics.CheckCapsule(
            capsuleCollider.bounds.center, new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y - 0.1f, capsuleCollider.bounds.center.z),
            0.18f);
    }

    private void Move()
    {
        // Update the movement direction based on input
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * inputVector.y + right * inputVector.x;
        moveDirection *= moveSpeed;



        //moveDirection.y = verticalVelocity;

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Apply gravity
            if (characterController.isGrounded)
            {
                verticalVelocity = -10f; // Small negative value to keep the character grounded

                // Jump
                if (jump)
                {
                    verticalVelocity = jumpForce;
                    jump = false;
                }
            }

            //else
            //{
            //    verticalVelocity += gravity * Time.deltaTime;
            //}
        }
    }
}
