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
    
    public void EndDodgeRoll()
    {
        player.isRolling = false;
    }

    public void StartLanding()
    {
        player.isLanding = true;
    }

    public void EndLanding()
    {
        player.isLanding = false;
    }
}
