using System;
using System.Collections;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    #region Variables

    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float activeTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    [SerializeField] private int normalBulletDamage = 1;
    
    private Rigidbody rb;
    private Vector3 targetDirection;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //is the collision within the whatDestroysBullet layerMask
        if ((whatDestroysBullet.value & (1 << other.gameObject.layer)) > 0)
        {
            //spawn particles
            //play sound FX
            //Screenshake
            
            //Damage Objects
            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(normalBulletDamage);
            }
            
            //Destroy the bullet
            gameObject.SetActive(false);
        }
    }

    #endregion
    
    #region Bullet Methods

    public void SetDirection(Vector3 direction)
    {
        targetDirection = direction;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = new Vector3(-0.8f, -0.4f, 0.2f);
        }
        SetVelocity();
        StartCoroutine(SetObjectInactive());
    }
    
    private void SetVelocity()
    {
        rb.velocity = targetDirection * normalBulletSpeed;
    }

    private IEnumerator SetObjectInactive()
    {
        float elapsedTime = 0f;

        while (elapsedTime < activeTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        elapsedTime = 0f;
        gameObject.SetActive(false);
    }

    #endregion
    
}
