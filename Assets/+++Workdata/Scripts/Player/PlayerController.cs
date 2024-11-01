using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpPower = 5f;
    public float rollSpeed = 10f;
    public LayerMask groundLayer;

    private Vector2 moveInput;
    private Vector2 mousePosition;
    private Rigidbody rb;
    private GameInput inputActions;
    private InputAction moveAction;
    private InputAction mouseAction;
    private float speed;
    private bool hasDoubleJump = true;
    private bool isRolling = false;
    public bool isShooting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new GameInput();
        moveAction = inputActions.Player.Move;
        mouseAction = inputActions.Player.MousePosition;
        speed = normalSpeed;
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        mousePosition = mouseAction.ReadValue<Vector2>();
        isGrounded();
    }

    private void FixedUpdate()
    {
        if (!isRolling)
        {
            rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Jump.performed += OnJump;
        
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;

        inputActions.Player.DodgeRoll.performed += OnDodgeRoll;

        inputActions.Player.Shoot.performed += OnShoot;
        inputActions.Player.Shoot.canceled += OnShoot;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Jump.performed -= OnJump;
        
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;

        inputActions.Player.DodgeRoll.performed -= OnDodgeRoll;
        
        inputActions.Player.Shoot.performed -= OnShoot;
        inputActions.Player.Shoot.canceled -= OnShoot;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode.Impulse);
        }
        else if (context.performed && !isGrounded() && hasDoubleJump)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode.Impulse);
            hasDoubleJump = false;
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            speed = sprintSpeed;
        }

        if (context.canceled)
        {
            speed = normalSpeed;
        }
    }

    private void OnDodgeRoll(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            isRolling = true;
            rb.AddForce(new Vector2(rollSpeed, rb.velocity.y), ForceMode.Impulse);
        }   
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isShooting = true;
        }

        if (context.canceled)
        {
            isShooting = false;
        }
    }

    private bool isGrounded()
    {
        bool hitGround = Physics.Raycast(transform.position, Vector3.down, 0.7f, groundLayer);

        if (hitGround)
        {
            hasDoubleJump = true;
        }

        return hitGround;
    }
}
