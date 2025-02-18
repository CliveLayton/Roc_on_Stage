using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class SwordBehavior : MonoBehaviour
{
    #region Variables

    [SerializeField] private LayerMask whatSwordDamages;
    [SerializeField] private float slowAmount = 0.1f;
    [SerializeField] private float slowTimeOnHit = 0.01f;
    [SerializeField] private VisualEffect slashEffect;
    private int swordDamage = 1;
    private CinemachineImpulseSource cmImpulse;

    #endregion

    #region Unity Methods

    private void Start()
    {
        cmImpulse = FindObjectOfType<CinemachineImpulseSource>();
        swordDamage = GameStateManager.instance.playerSwordDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        //is the collision within the whatSwordDamage layerMask
        if ((whatSwordDamages.value & (1 << other.gameObject.layer)) > 0)
        {
            //spawn particles
            //play sound FX
            //Screenshake
            slashEffect.Play();
            
            
            //Damage Enemy
            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                StartCoroutine(SlowTimeShortly());
                cmImpulse.GenerateImpulse();
                iDamageable.Damage(swordDamage);
            }
            
        }
    }

    #endregion

    #region SwordBehavior Methods

    /// <summary>
    /// slow the time for a short time
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowTimeShortly()
    {
        Time.timeScale = slowAmount;
        yield return new WaitForSeconds(slowTimeOnHit);
        Time.timeScale = 1f;
    }

    #endregion
}
