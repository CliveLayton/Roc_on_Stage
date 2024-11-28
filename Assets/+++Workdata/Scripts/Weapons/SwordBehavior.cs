using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    #region Variables

    [SerializeField] private LayerMask whatSwordDamages;
    [SerializeField] private int swordDamage = 1;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        //is the collision within the whatSwordDamage layerMask
        if ((whatSwordDamages.value & (1 << other.gameObject.layer)) > 0)
        {
            //spawn particles
            //play sound FX
            //Screenshake
            
            //Damage Enemy
            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(swordDamage);
            }
        }
    }

    #endregion
}
