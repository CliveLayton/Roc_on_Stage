using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public EnemyAimAndShoot enemyShooter;
    
    public void Throw()
    {
        enemyShooter.ThrowBullet();
    }
}
