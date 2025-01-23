using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    #region Variables

    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Cage blockade;
    [SerializeField] private bool hasBlockade;

    private bool checkForNull = false;

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (checkForNull)
        {
            //GetLastRemainingEnemy();
            
            if (AllItemsNull())
            {
                if (hasBlockade)
                {
                    blockade.OpenCage();
                }
                this.gameObject.SetActive(false);
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            checkForNull = true;
            foreach (Enemy enemy in enemies)
            {
                enemy.targetTransform = other.transform;
                enemy.hasTarget = true;
            }
        }
    }

    #endregion

    #region EnemyDetection Methods

    /// <summary>
    /// check if all enemies are destroyed
    /// </summary>
    /// <returns></returns>
    private bool AllItemsNull()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// check if only one enemy is left
    /// </summary>
    private void GetLastRemainingEnemy()
    {
        Enemy lastEnemy = null;
        int nonNullCount = 0;

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                nonNullCount++;
                lastEnemy = enemy;

                if (nonNullCount > 1)
                {
                    return;
                }
            }
        }

        if (lastEnemy != null)
        {
            lastEnemy.dropKey = true;
        }
    }

    #endregion
}
