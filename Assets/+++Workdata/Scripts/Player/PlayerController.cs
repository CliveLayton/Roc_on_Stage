using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpPower = 5f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private GameInput inputActions;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new GameInput();
        moveAction = inputActions.Player.Move;
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Jump.performed -= OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode2D.Impulse);
        }
    }
}
