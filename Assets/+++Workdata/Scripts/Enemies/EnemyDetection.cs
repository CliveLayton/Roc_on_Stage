using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    #region Variables

    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Cage blockade;
    [SerializeField] private bool hasBlockade;
    
    public enum Music
    {
        Forest,
        Town,
        Castle,
        Boss
    }

    public Music music;

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
                switch (music)
                {
                    case Music.Forest:
                        MusicManager.Instance.PlayMusic(MusicManager.Instance.forestMusic, 0.5f);
                        break;
                    case Music.Town:
                        MusicManager.Instance.PlayMusic(MusicManager.Instance.townMusic, 0.5f);
                        break;
                    case Music.Castle:
                    case Music.Boss:
                        MusicManager.Instance.PlayMusic(MusicManager.Instance.castleMusic, 0.5f);
                        break;
                }

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
            MusicManager.Instance.PlayMusic(MusicManager.Instance.combatMusic, 0.5f);
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
