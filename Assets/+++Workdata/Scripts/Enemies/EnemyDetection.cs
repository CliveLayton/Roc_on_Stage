using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDetection : MonoBehaviour
{
    #region Variables

    [SerializeField] private Enemy[] enemies;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.targetTransform = other.transform;
                enemy.hasTarget = true;
                this.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region EnemyDetection Methods



    #endregion
}
