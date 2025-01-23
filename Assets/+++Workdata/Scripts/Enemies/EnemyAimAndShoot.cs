using System.Collections;
using UnityEngine;

public class EnemyAimAndShoot : MonoBehaviour
{
    #region Variables

    [SerializeField] private LayerMask layersToCover;
    [SerializeField] private float bulletSpeed;
    
    public Animator anim;
    
    private GameObject target;
    private Coroutine detectPlayerCoroutine;
    private bool isCovered;
    private Vector3 directionToPredictedPosition;
    private Enemy enemyBase;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        enemyBase = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            anim.SetBool("canThrow", true);
            detectPlayerCoroutine = StartCoroutine(DetectPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            anim.SetBool("canThrow", false);
            StopCoroutine(detectPlayerCoroutine);
        }
    }

    #endregion

    #region EnemyAimAndShoot Methods

    /// <summary>
    /// calculate the position where to throw the bulb and check if the target is covered
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (target == null) continue;
            
            float distanceToPlayer = Vector3.Distance(this.transform.position, target.transform.position);
            Vector3 directionToPlayer = (target.transform.position - this.transform.position).normalized;
            Vector3 predictedPosition = PredictPlayerPosition(distanceToPlayer);
            directionToPredictedPosition = (predictedPosition - this.transform.position).normalized;

            isCovered = IsPlayerCovered(directionToPlayer, distanceToPlayer);
            
        }
    }

    /// <summary>
    /// get a bulb from the objectpool and set the direction to move it
    /// </summary>
    public void ThrowBullet()
    {
        if (!isCovered && !enemyBase.isDying)
        {
            GameObject bulletInst = ObjectPooling.Instance.GetPooledObject(); 
            if (bulletInst != null)
            {
                bulletInst.transform.position = transform.position;
                bulletInst.transform.rotation = Quaternion.identity;
                bulletInst.SetActive(true);
                bulletInst.GetComponent<BulletBehavior>().SetDirection(directionToPredictedPosition);
            }
        }
    }

    /// <summary>
    /// check if the player is covered
    /// </summary>
    /// <param name="direction">direction to fire the raycast</param>
    /// <param name="distanceToTarget">distance to the target</param>
    /// <returns></returns>
    private bool IsPlayerCovered(Vector3 direction, float distanceToTarget)
    {
        RaycastHit[] hits = Physics.RaycastAll(this.transform.position, direction, distanceToTarget, layersToCover);

        foreach (var hit in hits)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// calculate the position the target would be when bullet would reach him
    /// </summary>
    /// <param name="distanceToPlayer">distance to the target</param>
    /// <returns></returns>
    private Vector3 PredictPlayerPosition(float distanceToPlayer)
    {
        Rigidbody playerRb = target.GetComponent<Rigidbody>();

        float timeToReachPlayer = distanceToPlayer / bulletSpeed;

        Vector3 predictedPosition = target.transform.position + playerRb.velocity * timeToReachPlayer;

        return predictedPosition;
    }

    #endregion
    
}
