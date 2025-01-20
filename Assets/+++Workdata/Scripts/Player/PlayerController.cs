using System;
using System.Collections;
using Cinemachine;
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
    [SerializeField] private float slowAmount = 0.1f;
    [SerializeField] private float slowTimeOnHit = 0.01f;
    public int maxHealth;
    public LayerMask groundLayer;
    public CinemachineVirtualCamera cm;
    public GameObject parrySymbol;
    public Vector2 moveInput;
    private Rigidbody rb;
    private Animator anim;
    public float speed;
    //private bool hasDoubleJump = true;
    //public bool isRolling = false;
    //private bool usedStompAttack = false;
    public bool isAttacking = false;
    public bool isCountering = false;
    private bool canCounter = false;
    public int attackID = 0;
    //private SpriteRenderer[] playerVisuals;
    private SpriteRenderer playerVisual;
    private int currentHealth;
    private bool allowDamage = true;
    public bool isDying = false;
    public bool isGameover = false;
    public float invincibleTime = 0.5f;
    private Material playerMaterial;
    private Quaternion targetRotation;
    private Vector3 enemyPos;
    private bool parryToRight = true;
    private HeartBarUI heartBar;
    private CinemachineImpulseSource cmImpulse;

    public float activeTime = 2f;

    //Mesh Trail Variables
    [Header("Mesh Related")] 
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;
    public GameObject playerCounterPrefab;

    [Header("Shader Related")]
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;
    
    private SpriteRenderer[] spriteRenderers;
    private Material[] counterMaterials;


    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerVisual = GetComponentInChildren<SpriteRenderer>();
        playerMaterial = GetComponentInChildren<SpriteRenderer>().material;

        speed = normalSpeed;
        currentHealth = maxHealth;

        switch (GameStateManager.instance.currentPlayerState)
        {
            case GameStateManager.PlayerState.Claws:
                attackID = 0;
                break;
            case GameStateManager.PlayerState.Stick:
                attackID = 1;
                break;
            case GameStateManager.PlayerState.Lance:
                attackID = 2;
                break;
        }
    }

    private void Start()
    {
        heartBar = FindObjectOfType<HeartBarUI>().GetComponent<HeartBarUI>();
        cmImpulse = FindObjectOfType<CinemachineImpulseSource>();
        heartBar.UpdateHearts(maxHealth);
        allowDamage = true;
    }

    private void FixedUpdate()
    {
        if (!isCountering && !isDying && !isGameover)
        {
            PlayerMovement();
        }
        else if (isCountering || isDying && !isGameover)
        {
            playerVisual.transform.Rotate(0, 10, 0 ,Space.Self);
        }
        else if (isGameover)
        {
            playerVisual.transform.Rotate(5, 0, 0 ,Space.Self);
        }
    }

    private void LateUpdate()
    {
        PlayerAnimations();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            parrySymbol.SetActive(true);
            canCounter = true;
            if (other.transform.position.x < transform.position.x)
            {
                enemyPos = other.transform.position + Vector3.left * 2;
                parryToRight = false;
            }
            else if (other.transform.position.x > transform.position.x)
            {
                enemyPos = other.transform.position + Vector3.right * 2;
                parryToRight = true;
            }
        }

        if (other.gameObject.CompareTag("Key"))
        {
            GameStateManager.instance.playerKeys += 1;
        }

        if (other.gameObject.CompareTag("Stick"))
        {
            GameStateManager.instance.currentPlayerState = GameStateManager.PlayerState.Stick;
            attackID = 1;
            GameStateManager.instance.playerSwordDamage = 2;
        }

        if (other.gameObject.CompareTag("Lance"))
        {
            GameStateManager.instance.currentPlayerState = GameStateManager.PlayerState.Lance;
            attackID = 2;
            GameStateManager.instance.playerSwordDamage = 3;
        }

        if (other.gameObject.CompareTag("Blockade"))
        {
            if (isAttacking && GameStateManager.instance.playerKeys > 0)
            {
                GameStateManager.instance.playerKeys -= 1;
                other.GetComponent<Cage>().OpenCage();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            parrySymbol.SetActive(false);
            canCounter = false;
        }
    }

    #endregion

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded() && !isDying && !isGameover)
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
        if (context.performed && !isAttacking && !isDying && !isGameover)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
    }

    public void OnCounter(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking && isGrounded() && canCounter && !isCountering && !isDying && !isGameover)
        {
            isCountering = true;
            MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.parry);
            //anim.SetTrigger("Counter");
            StartCoroutine(ParryMovement());
            StartCoroutine(ActivateTrail(0.6f));
            cm.m_LookAt = playerVisual.transform;
            Time.timeScale = 0.7f;
            StartCoroutine(InvincibleTime());
        }
        // else if (context.performed && !isAttacking && isGrounded() && canCounter && !isCountering && Mathf.Abs(playerVisual.transform.rotation.y) > 0.3f)
        // {
        //     isCountering = true;
        //     anim.SetTrigger("CounterMirror");
        //     StartCoroutine(ActivateTrail(0.6f));
        //     cm.m_LookAt = playerVisual.transform;
        //     Time.timeScale = 0.7f;
        //     StartCoroutine(InvincibleTime());
        // }
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

        // foreach (var sprite in playerVisuals)
        // {
        //     sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // }
        playerVisual.transform.rotation = Quaternion.Slerp(playerVisual.transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);
    }
    

    #endregion

    #region PlayerController Methods

    public void Damage(int damageAmount)
    {
        if (!allowDamage || isCountering || isDying)
        {
            return;
        }
        
        StartCoroutine(SlowTimeShortly());
        cmImpulse.GenerateImpulse();
        currentHealth -= damageAmount;
        heartBar.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            isDying = true;
            MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.gameOver);
            anim.SetTrigger("Die");
            allowDamage = false;
            return;
        }
        
        StartCoroutine(InvincibleTime());
        StartCoroutine(LerpBetweenColors());
    }

    private bool isGrounded()
    {
        bool hitGround = Physics.Raycast(transform.position, Vector3.down, 0.7f, groundLayer);

        return hitGround;
    }

    private IEnumerator InvincibleTime()
    {
        allowDamage = false;
        float elapsedTime = 0f;
        yield return new WaitForSeconds(0.4f);

        while (elapsedTime < invincibleTime)
        {
            elapsedTime += 0.2f;
            playerMaterial.SetFloat("_Alpha", 0.5f);
            yield return new WaitForSeconds(0.1f);
            playerMaterial.SetFloat("_Alpha", 1f);
            yield return new WaitForSeconds(0.1f);
        }
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

    private IEnumerator ParryMovement()
    {
        Vector3 startPos = this.transform.position;
        float distance = (enemyPos - startPos).magnitude;
        float totalTime = 0.5f;
        float parrySpeed = distance / totalTime;
        float time = 0f;
        
        while (time < totalTime)
        {
            time += Time.deltaTime;
            if (parryToRight)
            {
                startPos.x += parrySpeed * Time.deltaTime;
            }
            else
            {
                startPos.x -= parrySpeed * Time.deltaTime;
            }
            
            if (time < totalTime / 2)
            {
                startPos.z -= 0.04f;
            }
            else if(time > totalTime/2)
            {
                startPos.z += 0.04f;
            }

            transform.position = startPos;
            yield return null;
        }

        transform.position = enemyPos;
        isCountering = false;
        Time.timeScale = 1f;
    }
    
    private IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (spriteRenderers == null)
            {
                spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            }

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                // GameObject gObj = playerCounterPrefab;
                // gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
                GameObject gObj = Instantiate(playerCounterPrefab, positionToSpawn.position, positionToSpawn.rotation);
                
                SpriteRenderer mainsr =  gObj.GetComponent<SpriteRenderer>();
                SpriteRenderer secondSr = gObj.GetComponentInChildren<SpriteRenderer>();

                counterMaterials = new[] { mainsr.material, secondSr.material };

                StartCoroutine(AnimateMaterialFloat( counterMaterials, 0, shaderVarRate, shaderVarRefreshRate));
                
                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
    }

    private IEnumerator AnimateMaterialFloat(Material[] mats, float goal, float rate, float refreshRate)
    {
        foreach (Material mat  in mats)
        {
            float valueToAnimate = mat.GetFloat(shaderVarRef);
            
            while (valueToAnimate > goal)
            {
                valueToAnimate -= rate;
                mat.SetFloat(shaderVarRef, valueToAnimate);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
    
    private IEnumerator SlowTimeShortly()
    {
        Time.timeScale = slowAmount;
        yield return new WaitForSeconds(slowTimeOnHit);
        Time.timeScale = 1f;
    }

    #endregion

    #region Animation/Sound Methods

    private void PlayerAnimations()
    {
        anim.SetBool("isGrounded", isGrounded());
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        //anim.SetBool("isDodgeRolling", isRolling);
        //anim.SetBool("isStompAttacking", usedStompAttack);
        anim.SetInteger("ActionID", attackID);

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
