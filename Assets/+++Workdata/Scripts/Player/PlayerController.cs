using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamageable
{

    #region Variables

    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float inActionSpeed = 3f;
    public float jumpPower = 5f;
    //public float rollSpeed = 10f;
    //public float stompPower = 10f;
    public float rotationSpeed = 50f;
    public int maxHealth;
    public LayerMask groundLayer;

    public Vector2 moveInput;
    private Rigidbody rb;
    private Animator anim;
    public float speed;
    //private bool hasDoubleJump = true;
    //public bool isRolling = false;
    //private bool usedStompAttack = false;
    public bool isLanding = false;
    public bool isAttacking = false;
    public int attackID = 0;
    private SpriteRenderer[] playerVisuals;
    private int currentHealth;


    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerVisuals = GetComponentsInChildren<SpriteRenderer>();

        speed = normalSpeed;
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (!isLanding)
        {
            PlayerMovement();
        }
    }

    private void LateUpdate()
    {
        PlayerAnimations();
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
            anim.SetBool("isJumping", true);
        }
        /*else if (context.performed && !isGrounded() && hasDoubleJump)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode.Impulse);
            hasDoubleJump = false;
            anim.SetBool("isJumping", true);
        }*/
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

    /*public void OnDodgeRoll(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            isRolling = true;
            rb.AddForce(new Vector2(moveInput.x * rollSpeed, rb.velocity.y), ForceMode.Impulse);
        }
    }*/

    /*public void OnStompAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isGrounded() && !usedStompAttack)
        {
            usedStompAttack = true;
            rb.AddForce(new Vector2(0, - stompPower), ForceMode.Impulse);
        }
    }*/

    public void OnAttacking(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded() && !isAttacking)
        {
            isAttacking = true;
        }
        else if (context.performed && isGrounded() && isAttacking)
        {
            attackID = 1;
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
        
        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);
        
    }
    

    #endregion

    #region PlayerController Methods

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool isGrounded()
    {
        bool hitGround = Physics.Raycast(transform.position, Vector3.down, 0.7f, groundLayer);

        return hitGround;
    }

    #endregion

    #region Animation Methods

    private void PlayerAnimations()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        //anim.SetBool("isDodgeRolling", isRolling);
        //anim.SetBool("isStompAttacking", usedStompAttack);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("ActionID", attackID);

        if (isGrounded())
        {
            anim.SetBool("isLanding", true);
        }
        else
        {
            anim.SetBool("isLanding", false);
        }

        if (rb.velocity.y < 0)
        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    #endregion
    
}
