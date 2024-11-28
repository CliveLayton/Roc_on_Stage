using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    #region Variables

    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    [SerializeField] private int normalBulletDamage = 1;
    
    private Rigidbody rb;
    private Vector3 targetDirection;

    #endregion

    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        SetDestroyTime();
        
        SetVelocity();
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
            Destroy(gameObject);
        }
    }

    #endregion
    
    #region Bullet Methods

    public void SetDirection(Vector3 direction)
    {
        targetDirection = direction;
    }
    
    private void SetVelocity()
    {
        rb.velocity = targetDirection * normalBulletSpeed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

    #endregion
    
}
