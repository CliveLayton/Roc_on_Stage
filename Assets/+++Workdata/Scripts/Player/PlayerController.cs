using System;
using System.Collections;
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
    private bool allowDamage = true;
    public float invincibleTime = 0.5f;
    private Material playerMaterial;
    private Quaternion targetRotation;


    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerVisuals = GetComponentsInChildren<SpriteRenderer>();
        playerMaterial = GetComponentInChildren<SpriteRenderer>().material;

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
        if (context.performed && !isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
        else if (context.performed && isAttacking)
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
            targetRotation = Quaternion.identity;
           
        }
        else if (moveInput.x < 0)
        {
            targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
        }
        
        foreach (var sprite in playerVisuals)
        {
            sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);
        
    }
    

    #endregion

    #region PlayerController Methods

    public void Damage(int damageAmount)
    {
        if (!allowDamage)
        {
            return;
        }

        StartCoroutine(InvincibleTime());
        StartCoroutine(LerpBetweenColors());
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

    private IEnumerator InvincibleTime()
    {
        allowDamage = false;
        yield return new WaitForSeconds(invincibleTime);
        allowDamage = true;
    }
    
    private IEnumerator LerpBetweenColors()
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Color startColor = Color.black;
        Color endColor = Color.red;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / duration;
            playerMaterial.SetColor("_SpriteColor", Color.Lerp(startColor,endColor, time));
            yield return null;
        }
        
        playerMaterial.SetColor("_SpriteColor", endColor);
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / duration;
            playerMaterial.SetColor("_SpriteColor", Color.Lerp(endColor, startColor,time));
            yield return null;
        }
        
        playerMaterial.SetColor("_SpriteColor", startColor);
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
