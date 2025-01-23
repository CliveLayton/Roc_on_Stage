using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject playerLifeBar;
    [SerializeField] private CanvasGroup gameOverMenu;
    [SerializeField] private GameObject inGameUI;
    private CanvasGroup loadingPanel;

    #endregion
    
    #region Unity Methods

    private void Awake()
    {
        loadingPanel = GetComponent<CanvasGroup>();
    }

    #endregion
    
    #region LoadingScreen Methods

    /// <summary>
    /// enable the playerinput if there is a player in the scene and hide the loading screen
    /// </summary>
    public void HideLoadingScreen()
    {
        PlayerInputManager playerInput = FindObjectOfType<PlayerInputManager>();
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
        loadingPanel.HideCanvasGroup();
        inGameUI.SetActive(true);
    }

    /// <summary>
    /// stop current in game sfx sounds
    /// </summary>
    public void StopMusic()
    {
        MusicManager.Instance.StopInGameSFX();
    }
    
    /// <summary>
    /// play curtain open music
    /// </summary>
    public void CurtainOpenMusic()
    {
        MusicManager.Instance.PlayUISFX(MusicManager.Instance.curtainOpen);
    }

    /// <summary>
    /// start the music for loading screen and set lifebar active
    /// </summary>
    public void StartMusic()
    {
        gameOverMenu.HideCanvasGroup();
        if (GameStateManager.instance.currentState == GameStateManager.GameState.InGame)
        {
            playerLifeBar.SetActive(true);
        }
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.stageRevolving);
    }

    public void StartLoading()
    {
        PlayerInputManager playerInput = FindObjectOfType<PlayerInputManager>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
        inGameUI.SetActive(false);
        MusicManager.Instance.PlayUISFX(MusicManager.Instance.curtainClose);
    }

    #endregion
    
}
