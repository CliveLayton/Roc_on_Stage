using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    #region Variables

    [SerializeField] private Enemy[] enemies;
    //[SerializeField] private bool containsKey;

    //private bool checkForNull = false;

    #endregion

    #region Unity Methods

    // private void Update()
    // {
    //     if (checkForNull && containsKey)
    //     {
    //         GetLastRemainingEnemy();
    //         
    //         if (AllItemsNull())
    //         {
    //             this.gameObject.SetActive(false);
    //         } 
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //checkForNull = true;
            foreach (Enemy enemy in enemies)
            {
                enemy.targetTransform = other.transform;
                enemy.hasTarget = true;
            }
        }
    }

    #endregion

    #region EnemyDetection Methods

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
