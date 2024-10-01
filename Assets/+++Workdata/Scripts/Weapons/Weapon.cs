using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public MoverBase owner;

    /// <summary>
    /// sets the owner of this Weapon to the new owner with Moverbase Script
    /// </summary>
    /// <param name="owner"></param>
    public void GetEquipedBy(MoverBase owner)
    {
        this.owner = owner;
    }

    public virtual void StartShooting()
    {
        
    }

    public virtual void StopShooting()
    {
        
    }
}
