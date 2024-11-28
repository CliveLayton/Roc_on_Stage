using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour , IDamageable
{
    #region Variables

    [SerializeField] private int maxHealth = 5;
    // [SerializeField] private float outlineThickness = 5f;
    // [ColorUsage(showAlpha:true, hdr:true)]
    // [SerializeField] private Color outlineGlowColor;
    // [ColorUsage(showAlpha:true, hdr:true)]
    // [SerializeField] private Color resetOutlineColor;
    private NavMeshAgent agent;
    //private Material enemyMaterial;

    private int currentHealth;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //enemyMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        agent.SetDestination(FindObjectOfType<PlayerController>().gameObject.transform.position);

        this.transform.rotation = quaternion.identity;
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

    #endregion
    
    #region Enemy Methods

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion
   
}
