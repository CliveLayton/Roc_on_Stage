using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBasedWeapon : Weapon
{
    [Header("Ammo Based Weapon")] 
    public int bulletsPerMagazine;
    public int currentBullets;

    public float timeBetweenBullets = 0.5f;
    public float reloadTime;

    private bool isShooting = false;

    private float timeSinceLastBullet;

    private void Update()
    {
        timeSinceLastBullet += Time.deltaTime;

        if (!isShooting)
        {
            return;
        }

        if (timeSinceLastBullet < timeBetweenBullets)
        {
            return;
        }

        timeSinceLastBullet = 0;
        ShootBullet();
    }
    
    public virtual void ShootBullet()
    {
        
    }

    public override void StartShooting()
    {
        isShooting = true;
    }

    public override void StopShooting()
    {
        isShooting = false;
    }
}
