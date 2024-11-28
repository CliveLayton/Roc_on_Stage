using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public EnemyAimAndShoot enemyShooter;
    
    public void Throw()
    {
        enemyShooter.ThrowBullet();
    }
}
