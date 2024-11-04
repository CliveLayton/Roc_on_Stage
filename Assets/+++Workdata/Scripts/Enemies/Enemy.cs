using UnityEngine;

public class Enemy : MonoBehaviour , IDamageable
{
    #region Variables

    [SerializeField] private float maxHealth = 5f;

    private float currentHealth;

    #endregion

    #region Unity Methods

    private void Start()
    {
        currentHealth = maxHealth;
    }

    #endregion
    
    #region Enemy Methods

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion
   
}
