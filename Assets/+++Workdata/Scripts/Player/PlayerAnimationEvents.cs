using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimationEvents : MonoBehaviour
{
    #region Variables

    private PlayerController player;
    
    private HeartBarUI heartBar;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        heartBar = FindObjectOfType<HeartBarUI>().GetComponent<HeartBarUI>();
    }

    #endregion
    
    #region PlayerAnimationEvents Methods

    /// <summary>
    /// slows player for attacking
    /// </summary>
    public void InAttacking()
    {
        player.speed = player.inActionSpeed;
    }

    /// <summary>
    /// slows player for attacking and set is attacking to false
    /// </summary>
    public void OutAttack()
    {
        player.speed = player.inActionSpeed;
        player.isAttacking = false;
    }

    /// <summary>
    /// set countering to false, set time to normal and set player position to the visual position
    /// </summary>
    public void OutCounter()
    {
        player.isCountering = false;
        player.transform.position = this.transform.position;
        player.cm.m_LookAt = player.transform;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// play weapon sound
    /// </summary>
    public void WeaponSound()
    {
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.weaponAttacks[Random.Range(0,MusicManager.Instance.weaponAttacks.Length)]);
    }

    /// <summary>
    /// play claw sound
    /// </summary>
    public void ClawSound()
    {
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.clawAttack);
    }

    /// <summary>
    /// play footsteps
    /// </summary>
    public void FootStepsSound()
    {
        MusicManager.Instance.PlayFootsteps();
    }

    /// <summary>
    /// set isdying to false and gameover to true
    /// </summary>
    public void StopRotateOnDying()
    {
        player.isDying = false;
        player.isGameover = true;
    }

    /// <summary>
    /// open the game over screen and reset the heart bar, sets gameover to false
    /// </summary>
    public void DyingEnd()
    {
        FindObjectOfType<InGameUI>().OpenGameOverMenu();
        FindObjectOfType<InGameUI>().gameObject.SetActive(false);
        heartBar.UpdateHearts(3);
        player.isGameover = false;
    }

    #endregion

}
