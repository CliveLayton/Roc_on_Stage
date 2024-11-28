using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimAndShoot : MonoBehaviour
{
    #region Variables

    [SerializeField] private LayerMask layersToCover;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    private GameObject bulletInst;
    private GameObject target;
    private Coroutine detectPlayerCoroutine;
    public Animator anim;
    private bool isCovered;
    private Vector3 directionToPredictedPosition;

    #endregion

    #region Unity Methods

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

    public void ThrowBullet()
    {
        if (!isCovered)
        {
            bulletInst = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bulletInst.GetComponent<BulletBehavior>().SetDirection(directionToPredictedPosition);
        }
    }

    private bool IsPlayerCovered(Vector3 direction, float distanceToTarget)
    {
        RaycastHit[] hits = Physics.RaycastAll(this.transform.position, direction, distanceToTarget, layersToCover);

        foreach (var hit in hits)
        {
            return true;
        }

        return false;
    }

    private Vector3 PredictPlayerPosition(float distanceToPlayer)
    {
        Rigidbody playerRb = target.GetComponent<Rigidbody>();

        float timeToReachPlayer = distanceToPlayer / bulletSpeed;

        Vector3 predictedPosition = target.transform.position + playerRb.velocity * timeToReachPlayer;

        return predictedPosition;
    }

    #endregion
    
}
