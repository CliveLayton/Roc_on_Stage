using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUI : MonoBehaviour
{
    #region Variables
    
    //all objects in the InGameHUD
    [SerializeField] private CanvasGroup optionMenu;
    [SerializeField] private CanvasGroup controlsMenu;
    [SerializeField] private CanvasGroup creditsMenu;
    [SerializeField] private CanvasGroup gameOverMenu;
    [SerializeField] private GameObject playerLifeBar;

    private PlayerInputManager playerInput;
    private GameInput inputActions;

    //bool to check if player may open menu
    public bool menuActive = true;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        inputActions = new GameInput();
    }

    private void Start()
    {
        GameStateManager.instance.onStateChanged += OnStateChange;
        if (GameStateManager.instance.currentState == GameStateManager.GameState.InMainMenu)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.UI.PauseGame.performed += PauseGame;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.UI.PauseGame.performed -= PauseGame;
    }

    #endregion

    #region InGameUI Methods

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed && menuActive &&
            GameStateManager.instance.currentState != GameStateManager.GameState.InMainMenu &&
            optionMenu.alpha == 0)
        {
            OpenInGameUI();
        }
        else if (context.performed && GameStateManager.instance.currentState != GameStateManager.GameState.InMainMenu &&
                 Math.Abs(optionMenu.alpha - 1) < 0.1f)
        {
            CloseInGameUI();
        }
    }

    private void OnStateChange(GameStateManager.GameState newState)
    {
        //we toggle the availability of the inGame menu whenever the game state changes
        bool isInGame = newState != GameStateManager.GameState.InMainMenu;
        gameObject.SetActive(isInGame);
    }

    /// <summary>
    /// open the ingame ui, unlock cursor and stop the time
    /// </summary>
    public void OpenInGameUI()
    {
        if (GameStateManager.instance.currentState == GameStateManager.GameState.InGame)
        {
            playerInput = FindObjectOfType<PlayerInputManager>().gameObject.GetComponent<PlayerInputManager>();
            playerInput.enabled = false;
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        MusicManager.Instance.PlayMusic(MusicManager.Instance.pauseMenuMusic, 0.1f);
        
        optionMenu.ShowCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.HideCanvasGroup();
    }

    /// <summary>
    /// close the ingame ui, set time to normal and locks cursor
    /// </summary>
    public void CloseInGameUI()
    {
        optionMenu.HideCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.HideCanvasGroup();

        if (GameStateManager.instance.currentState == GameStateManager.GameState.InGame)
        {
            playerInput.enabled = true;
        }
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        
        //get current scene at set music back to scene music
        switch (LoadSceneManager.instance.currentScene)
        {
            case GameStateManager.level1SceneName:
                MusicManager.Instance.PlayMusic(MusicManager.Instance.forestMusic, 0.1f);
                break;
            case GameStateManager.level2SceneName:
                MusicManager.Instance.PlayMusic(MusicManager.Instance.townMusic, 0.1f);
                break;
            case GameStateManager.level3SceneName:
            case GameStateManager.level4SceneName:
                MusicManager.Instance.PlayMusic(MusicManager.Instance.castleMusic, 0.1f);
                break;
        }
    }

    /// <summary>
    /// reload the current scene, set time to normal and lock cursor 
    /// </summary>
    public void ReloadScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameStateManager.instance.LoadNewGameplayScene(LoadSceneManager.instance.currentScene);
    }

    /// <summary>
    /// open the game over menu, stop time and unlock cursor
    /// </summary>
    public void OpenGameOverMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        gameOverMenu.ShowCanvasGroup();
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// loads the main menu and close the in game ui
    /// </summary>
    public void GoToMainMenu()
    {
        CloseInGameUI();
        playerLifeBar.SetActive(false);
        GameStateManager.instance.GoToMainMenu(false);
    }

    /// <summary>
    /// open options menu
    /// </summary>
    public void OpenOptionsMenu()
    {
        optionMenu.ShowCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.HideCanvasGroup();
    }

    /// <summary>
    /// open controls menu
    /// </summary>
    public void OpenControlsMenu()
    {
        optionMenu.HideCanvasGroup();
        controlsMenu.ShowCanvasGroup();
        creditsMenu.HideCanvasGroup();
    }

    /// <summary>
    /// open credits menu
    /// </summary>
    public void OpenCreditsMenu()
    {
        optionMenu.HideCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.ShowCanvasGroup();
    }

    #endregion
}
