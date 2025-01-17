using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController player;
    
    private InGameUI inGameUI;
    private HeartBarUI heartBar;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        heartBar = FindObjectOfType<HeartBarUI>().GetComponent<HeartBarUI>();
        inGameUI = FindObjectOfType<InGameUI>().GetComponent<InGameUI>();
    }

    /*public void EndDodgeRoll()
    {
        player.isRolling = false;
    }*/

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

    public void WeaponSound()
    {
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.weaponAttacks[Random.Range(0,MusicManager.Instance.weaponAttacks.Length)]);
    }

    public void ClawSound()
    {
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.clawAttack);
    }

    public void FootStepsSound()
    {
        MusicManager.Instance.PlayFootsteps();
    }

    public void StopRotateOnDying()
    {
        player.isDying = false;
        player.isGameover = true;
    }

    public void DyingEnd()
    {
        inGameUI.OpenGameOverMenu();
        heartBar.UpdateHearts(3);
        player.isGameover = false;
    }
}
