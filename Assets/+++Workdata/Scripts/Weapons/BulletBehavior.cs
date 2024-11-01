using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    [SerializeField] private float normalBulletDamage = 1f;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        SetDestroyTime();
        
        SetStraightVelocity();
    }

    private void OnTriggerEnter(Collider other)
    {
        //is the collision within the whatDestroysBullet layerMask
        if ((whatDestroysBullet.value & (1 << other.gameObject.layer)) > 0)
        {
            //spawn particles
            //play sound FX
            //Screenshake
            
            //Damage Enemy
            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                //damage enemy
                iDamageable.Damage(normalBulletDamage);
            }
            
            //Destroy the bulleta
            Destroy(gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * normalBulletSpeed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }
}
