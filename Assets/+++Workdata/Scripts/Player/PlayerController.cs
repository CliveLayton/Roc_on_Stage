using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    #region Variables

    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpPower = 5f;
    public float rollSpeed = 10f;
    public float stompPower = 10f;
    public float rotationSpeed = 50f;
    public LayerMask groundLayer;

    public Vector2 moveInput;
    private Rigidbody rb;
    private float speed;
    private bool hasDoubleJump = true;
    private bool isRolling = false;
    private bool usedStompAttack = false;
    private SpriteRenderer[] playerVisuals;


    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerVisuals = GetComponentsInChildren<SpriteRenderer>();

        speed = normalSpeed;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    #endregion

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
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

    public void OnSprint(InputAction.CallbackContext context)
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

    public void OnDodgeRoll(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            isRolling = true;
            rb.AddForce(new Vector2(moveInput.x * rollSpeed, rb.velocity.y), ForceMode.Impulse);
        }
    }

    public void OnStompAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isGrounded() && !usedStompAttack)
        {
            usedStompAttack = true;
            rb.AddForce(new Vector2(0, - stompPower), ForceMode.Impulse);
        }
    }

    #endregion

    #region Player Movement

    private void PlayerMovement()
    {
        if (moveInput.x > 0)
        {
            foreach (var sprite in playerVisuals)
            {
                sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            }
        }
        else if (moveInput.x < 0)
        {
            foreach (var sprite in playerVisuals)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
                
                sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        if (!isRolling)
        {
            rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);
        }
    }
    

    #endregion
   

    private bool isGrounded()
    {
        bool hitGround = Physics.Raycast(transform.position, Vector3.down, 0.7f, groundLayer);

        if (hitGround)
        {
            hasDoubleJump = true;
            usedStompAttack = false;
        }

        return hitGround;
    }
}
