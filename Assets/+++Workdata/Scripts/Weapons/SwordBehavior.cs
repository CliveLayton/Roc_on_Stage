using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    #region Variables

    [SerializeField] private LayerMask whatSwordDamages;
    public int swordDamage = 1;
    private CinemachineImpulseSource cmImpulse;

    #endregion

    #region Unity Methods

    private void Start()
    {
        cmImpulse = FindObjectOfType<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //is the collision within the whatSwordDamage layerMask
        if ((whatSwordDamages.value & (1 << other.gameObject.layer)) > 0)
        {
            //spawn particles
            //play sound FX
            //Screenshake
            StartCoroutine(SlowTimeShortly());
            cmImpulse.GenerateImpulse();
            
            //Damage Enemy
            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(swordDamage);
            }
            
        }
    }

    #endregion

    #region SwordBehavior Methods

    private IEnumerator SlowTimeShortly()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1f;
    }

    #endregion
}
