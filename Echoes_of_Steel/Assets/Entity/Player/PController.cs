using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private UnityEvent<Vector2> onMoveEvent;

    private void OnEnable()
    {
        moveAction.action.Enable();
        moveAction.action.performed += OnMove;
    }

    private void OnDisable()

    {
        moveAction.action.Disable();
        moveAction.action.performed -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        onMoveEvent.Invoke(moveInput);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
