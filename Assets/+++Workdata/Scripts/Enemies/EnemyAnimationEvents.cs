using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyAimAndShoot enemyShooter;

    private void Awake()
    {
        enemyShooter = GetComponentInParent<EnemyAimAndShoot>();
    }

    public void Throw()
    {
        enemyShooter.ThrowBullet();
    }
}
