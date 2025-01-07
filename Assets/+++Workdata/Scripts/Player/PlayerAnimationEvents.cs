using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }
    
    /*public void EndDodgeRoll()
    {
        player.isRolling = false;
    }*/

    public void StartLanding()
    {
        player.isLanding = true;
    }

    public void EndLanding()
    {
        player.isLanding = false;
    }

    public void InAttacking()
    {
        player.speed = player.inActionSpeed;
    }

    public void OutAttack()
    {
        player.speed = player.inActionSpeed;
        player.isAttacking = false;
    }

    public void OutCounter()
    {
        player.isCountering = false;
        player.transform.position = this.transform.position;
        player.cm.m_LookAt = player.transform;
        Time.timeScale = 1f;
    }
}
