using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PController : MonoBehaviour
{
    public Transform cameraTransform;
    public LayerMask Ground;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool isGrounded;

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

    //Creates a capsule below the player to check if he is grounded
    public bool GroundCheck()
    {
        if (Physics.CheckCapsule(capsuleCollider.bounds.center, new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y - 0.1f, capsuleCollider.bounds.center.z), 0.18f, Ground))
        {
            return isGrounded;
        }
        else 
        {
            return isGrounded = false;
        }
    }

    private void Move()
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //// Apply gravity
            //if (characterController.isGrounded)
            //{
            //    verticalVelocity = -10f; // Small negative value to keep the character grounded

            //    // Jump
            //    if (jump)
            //    {
            //        verticalVelocity = jumpForce;
            //        jump = false;
            //    }
            //}

            //else
            //{
            //    verticalVelocity += gravity * Time.deltaTime;
            //}
        }
    }
}
