using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour , IDamageable
{
    #region Variables

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockbackPower = 4f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject stickPrefab;
    [SerializeField] private GameObject lancePrefab;
    [SerializeField] private bool isBoss1;
    [SerializeField] private bool isBoos2;
    // [SerializeField] private float outlineThickness = 5f;
    // [ColorUsage(showAlpha:true, hdr:true)]
    // [SerializeField] private Color outlineGlowColor;
    // [ColorUsage(showAlpha:true, hdr:true)]
    // [SerializeField] private Color resetOutlineColor;
    public bool hasTarget = false;
    public Transform targetTransform;
    public bool dropKey = false;
    
    private NavMeshAgent agent;
    //private Material enemyMaterial;
    private Transform visualChild;

    private int currentHealth;
    private bool isDying = false;
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
        //enemyMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (hasTarget && !isDying && !player.isCountering)
        {
            agent.SetDestination(targetTransform.position);
        }
        else if (player.isCountering && !isDying)
        {
            StartCoroutine(EnemyStunned(2f));
        }

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

    /*private void OnMouseEnter()
    {
        enemyMaterial.SetFloat("_Thickness", outlineThickness);
        enemyMaterial.SetColor("_OutlineColor", outlineGlowColor);
    }

    private void OnMouseExit()
    {
        enemyMaterial.SetFloat("_Thickness", 0f);
        enemyMaterial.SetColor("_OutlineColor", resetOutlineColor);
    }*/

    private void OnCollisionEnter(Collision other)
    {
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
        currentHealth -= damageAmount;
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.damageEnemy);

        if (currentHealth <= 0)
        {
            isDying = true;
            col.enabled = false;
            StartCoroutine(EnemyDying());
        }

        StartCoroutine(EnemyStunned(0.5f));
        StartCoroutine(LerpBetweenColors());
    }

    private IEnumerator EnemyStunned(float stunTime)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
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

    private IEnumerator EnemyDying()
    {
        while (transform.position.y < -2f)
        {
            transform.position += new Vector3(0,1,-1) * (2 * Time.deltaTime);
            yield return null;
        }

        if (dropKey)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }

        if (isBoss1)
        {
            Instantiate(stickPrefab, transform.position, Quaternion.identity);
        }

        if (isBoos2)
        {
            Instantiate(lancePrefab, transform.position, Quaternion.identity);
        }

        while (transform.position.y > -5f)
        {
            transform.position -= new Vector3(0,1,2) * (5 * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    #endregion
   
}
