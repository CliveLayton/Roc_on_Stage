using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamageable
{
    #region Variables

    //Inspector Variables
    [Header("Player Behavior Variables")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float slowAmount = 0.1f;
    [SerializeField] private float slowTimeOnHit = 0.01f;
    [SerializeField] private int maxHealth;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject parrySymbol;
    [SerializeField] private Volume volume;
    
    //public Variables
    [Header("Public Variables for other Scripts")]
    public float inActionSpeed = 3f;
    public bool isAttacking = false;
    public bool isCountering = false;
    public bool isDying = false;
    public bool isGameover = false;
    public float speed;
    public Vector2 moveInput;
    public CinemachineVirtualCamera cm;

    //Mesh Trail Variables
    [Header("Mesh Related")] 
    [SerializeField] private float meshRefreshRate = 0.1f;
    [SerializeField] private float meshDestroyDelay = 3f;
    [SerializeField] private Transform positionToSpawn;
    [SerializeField] private GameObject playerCounterPrefab;

    [Header("Shader Related")]
    [SerializeField] private string shaderVarRef;
    [SerializeField] private float shaderVarRate = 0.1f;
    [SerializeField] private float shaderVarRefreshRate = 0.05f;
    
    private SpriteRenderer[] spriteRenderers;
    private Material[] counterMaterials;
    
    //private Variables
    private int attackID = 0;
    private int currentHealth;
    private bool canCounter = false;
    private bool allowDamage = true;
    private bool parryToRight = true;
    private float invincibleTime = 0.5f;
    private float vignetteValue = 0f;
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer playerVisual;
    private Material playerMaterial;
    private Quaternion targetRotation;
    private Vector3 enemyPos;
    private HeartBarUI heartBar;
    private CinemachineImpulseSource cmImpulse;
    private Vignette vignette;

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

        //set the current player state
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
        
        //ensure the volume profile is unique and editable
        if (volume == null)
        {
            Debug.LogError("Volume is not assigned");
            return;
        }
        
        //Get references to the volume effects
        if (volume.profile.TryGet(out Vignette vignetteEffect))
        {
            vignette = vignetteEffect;
        }
        
        vignette.intensity.Override(vignetteValue);
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
        if (other.gameObject.CompareTag("Parryable"))
        {
            parrySymbol.SetActive(true);
            canCounter = true;
            
            //check if the enemy is left or right to the player to set the parry correct
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

        if (other.gameObject.CompareTag("Heart"))
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += 1;
                heartBar.UpdateHearts(currentHealth);
            }
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
            if (GameStateManager.instance.playerKeys > 0)
            {
                other.GetComponent<Cage>().OpenCage();
            }
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (GameStateManager.instance.playerKeys > 0)
            {
                GameStateManager.instance.playerKeys -= 1;
                GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level4SceneName);
                MusicManager.Instance.PlayMusic(MusicManager.Instance.castleMusic, 0.1f);
                allowDamage = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Parryable"))
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

    /// <summary>
    /// set rotation of the player and velocity
    /// </summary>
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
        
        playerVisual.transform.rotation = Quaternion.Slerp(playerVisual.transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);
    }
    
    #endregion

    #region PlayerController Methods

    /// <summary>
    /// apply damage to player and start methods for player feedback
    /// </summary>
    /// <param name="damageAmount"></param>
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

    public void DenyParry()
    {
        canCounter = false;
        parrySymbol.SetActive(false);
    }

    /// <summary>
    /// check if the player is on the ground
    /// </summary>
    /// <returns></returns>
    private bool isGrounded()
    {
        bool hitGround = Physics.Raycast(transform.position, Vector3.down, 0.7f, groundLayer);

        return hitGround;
    }

    /// <summary>
    /// set allowDamage to false and set player material to blink for a short time, after that set allowDamage back to true
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// lerp between two colors on the material to visualize damage 
    /// </summary>
    /// <returns></returns>
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
            vignette.intensity.value = Mathf.Lerp(vignetteValue, 0.5f, time);
            yield return null;
        }
        
        playerMaterial.SetColor("_SpriteColor", endColor);
        vignette.intensity.value = 0.5f;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / duration;
            playerMaterial.SetColor("_SpriteColor", Color.Lerp(endColor, startColor,time));
            vignette.intensity.value = Mathf.Lerp(0.5f, vignetteValue, time);
            yield return null;
        }
        
        playerMaterial.SetColor("_SpriteColor", startColor);
        vignette.intensity.value = vignetteValue;
    }

    /// <summary>
    /// calculate parry movement to the back of an enemy and slow time while moving
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// instantiate parry Objects of player to visualize the parry and destroy them after a short time
    /// </summary>
    /// <param name="timeActive"></param>
    /// <returns></returns>
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

    /// <summary>
    /// set the alpha of the parry material to zero over time
    /// </summary>
    /// <param name="mats">the materials</param>
    /// <param name="goal">end value</param>
    /// <param name="rate">how much you substract each refreshrate</param>
    /// <param name="refreshRate"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// slows time shortly
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowTimeShortly()
    {
        Time.timeScale = slowAmount;
        yield return new WaitForSeconds(slowTimeOnHit);
        Time.timeScale = 1f;
    }

    #endregion

    #region Animation/Sound Methods

    /// <summary>
    /// set animation variables 
    /// </summary>
    private void PlayerAnimations()
    {
        anim.SetBool("isGrounded", isGrounded());
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
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
