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

    private PlayerController player;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        visualChild = this.transform.GetChild(0);
        col = GetComponent<BoxCollider>();
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
            agent.isStopped = false;
            agent.SetDestination(targetTransform.position);
        }
        else if (player.isCountering && !isDying)
        {
            agent.isStopped = true;
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

        if (currentHealth <= 0)
        {
            isDying = true;
            col.enabled = false;
            StartCoroutine(EnemyDying());
        }
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

        while (transform.position.y > -5f)
        {
            transform.position -= new Vector3(0,1,2) * (5 * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    #endregion
   
}
