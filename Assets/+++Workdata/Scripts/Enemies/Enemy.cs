using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour , IDamageable
{
    #region Variables

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockbackPower = 4f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isBoss1;
    [SerializeField] private bool isBoos2;
    [SerializeField] private bool isKnight;
    [SerializeField] private float attackThreshold = 0.0f; //threshold for determine what is behind the enemy
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject stickPrefab;
    [SerializeField] private GameObject lancePrefab;
    [SerializeField] private GameObject heartPrefab;
    
    public bool hasTarget = false;
    public Transform targetTransform;
    public bool dropKey = false;
    public bool dropHeart = false;
    public bool isDying = false;

    private int currentHealth;
    private NavMeshAgent agent;
    private Transform visualChild;
    private BoxCollider col;
    private Material enemyMaterial;
    private PlayerController player;
    

    #endregion

    #region Unity Methods

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        visualChild = this.transform.GetChild(0);
        col = GetComponent<BoxCollider>();
        enemyMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        //let the enemy work towards the player or stop him
        if (hasTarget && !isDying && !player.isCountering)
        {
            agent.SetDestination(targetTransform.position);
        }
        else if (player.isCountering && !isDying)
        {
            StartCoroutine(EnemyStunned(2f));
        }

        //rotate the sprite of the enemy
        if (!isDying && !player.isCountering)
        {
            if (agent.velocity.x <= 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
                visualChild.transform.rotation = Quaternion.Slerp(visualChild.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else if (agent.velocity.x > 0)
            {
                visualChild.transform.rotation = Quaternion.Slerp(visualChild.transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            }
        }
        else if (isDying)
        {
            agent.enabled = false;
            transform.Rotate(0, 10, 0 ,Space.Self);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //if enemy hit player, damage the player and get a knockback 
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
            if (idamageable != null)
            {
                idamageable.Damage(1);
                Vector3 directionToPlayer = (other.transform.position - this.transform.position).normalized;
                agent.velocity = knockbackPower * -directionToPlayer;
            }
        }
    }

    #endregion
    
    #region Enemy Methods

    public void Damage(int damageAmount)
    {
        //if the enemy is a knight only get damage, if the attacker is behind it 
        if (isKnight)
        {
            //get the direction from the enemy to the attacker
            Vector3 directionToAttacker = (targetTransform.position - visualChild.position).normalized;

            float dotProduct = Vector3.Dot(visualChild.right, directionToAttacker);
            
            //Check if the attacker is behind the enemy
            if (dotProduct >= attackThreshold)
            {
                return;
            }
        }
        
        currentHealth -= damageAmount;
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.damageEnemy);

        if (currentHealth <= 0)
        {
            isDying = true;
            col.enabled = false;
            StartCoroutine(EnemyDying());
            return;
        }

        StartCoroutine(EnemyStunned(0.5f));
        StartCoroutine(LerpBetweenColors());
    }

    /// <summary>
    /// stop the agent for a short time
    /// </summary>
    /// <param name="stunTime">time the enemy can't move</param>
    /// <returns></returns>
    private IEnumerator EnemyStunned(float stunTime)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(stunTime);
        if (!isDying)
        {
            agent.isStopped = false; 
        }
    }
    
    /// <summary>
    /// lerp between two colors for damage effect
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
            enemyMaterial.SetColor("_SpriteColor", Color.Lerp(startColor,endColor, time));
            yield return null;
        }
        
        enemyMaterial.SetColor("_SpriteColor", endColor);
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / duration;
            enemyMaterial.SetColor("_SpriteColor", Color.Lerp(endColor, startColor,time));
            yield return null;
        }
        
        enemyMaterial.SetColor("_SpriteColor", startColor);
    }

    /// <summary>
    /// let the enemy flip off the stage and if they should drop a item, let them drop it on the ground
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyDying()
    {
        while (transform.position.y < -2f)
        {
            transform.position += new Vector3(0,1,-1) * (2 * Time.deltaTime);
            yield return null;
        }

        Vector3 dropPoint = transform.position;
        if (Physics.Raycast(transform.position, Vector3.down, out var hit,Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            dropPoint = hit.point;
        }

        if (dropKey)
        {
            Instantiate(keyPrefab, dropPoint, Quaternion.identity);
        }
        else if (dropHeart)
        {
            Instantiate(heartPrefab, dropPoint, Quaternion.identity);
        }
        else if (isBoss1)
        {
            Instantiate(stickPrefab, dropPoint, Quaternion.identity);
        }
        else if (isBoos2)
        {
            Instantiate(lancePrefab, dropPoint, Quaternion.identity);
        }

        targetTransform.GetComponent<PlayerController>().DenyParry();
        
        while (transform.position.y > -5f)
        {
            transform.position -= new Vector3(0,1,2) * (5 * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }


    #endregion
   
}
