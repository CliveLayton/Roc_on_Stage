using UnityEngine;

public class Cage : MonoBehaviour
{
    #region Variables

    [SerializeField] private Animator npcAnim;
    private PlayerInputManager playerInput;
    private Animator anim;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = FindObjectOfType<PlayerInputManager>();
    }

    #endregion

    #region Cage Methods

    /// <summary>
    /// set the trigger for the animation to open cage
    /// </summary>
    public void OpenCage()
    {
        anim.SetTrigger("Open");
    }

    /// <summary>
    /// disable playerinput and show loading screen
    /// </summary>
    public void ShowLoadingScreen()
    {
        playerInput.enabled = false;
        LoadSceneManager.instance.ShowLoadingScreen();
    }

    /// <summary>
    /// enable playerinput and hide loading screen
    /// </summary>
    public void HideLoadingScreen()
    {
        playerInput.enabled = true;
        LoadSceneManager.instance.OpenLoadingScreen();
    }

    /// <summary>
    /// count one up for npcCounter and trigger happy animation for npc
    /// </summary>
    public void HappyNpc()
    {
        GameStateManager.instance.npcCounter += 1;
        GameStateManager.instance.playerKeys -= 1;
        npcAnim.SetTrigger("Happy");
    }

    /// <summary>
    /// count player keys 1 down
    /// </summary>
    public void UsedKey()
    {
        GameStateManager.instance.playerKeys -= 1;
    }

    #endregion
    
}
