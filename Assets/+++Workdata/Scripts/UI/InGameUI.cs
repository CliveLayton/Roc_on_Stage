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

    public void OpenInGameUI()
    {
        if (GameStateManager.instance.currentState == GameStateManager.GameState.InGame)
        {
            playerInput = FindObjectOfType<PlayerInputManager>().gameObject.GetComponent<PlayerInputManager>();
            playerInput.enabled = false;
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        
        optionMenu.ShowCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.HideCanvasGroup();
    }

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
    }

    public void ReloadScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameStateManager.instance.LoadNewGameplayScene(LoadSceneManager.instance.currentScene);
    }

    public void OpenGameOverMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        gameOverMenu.ShowCanvasGroup();
        Time.timeScale = 0f;
    }
    
    public void GoToMainMenu()
    {
        GameStateManager.instance.GoToMainMenu(false);
        CloseInGameUI();
    }

    public void OpenOptionsMenu()
    {
        optionMenu.ShowCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.HideCanvasGroup();
    }

    public void OpenControlsMenu()
    {
        optionMenu.HideCanvasGroup();
        controlsMenu.ShowCanvasGroup();
        creditsMenu.HideCanvasGroup();
    }

    public void OpenCreditsMenu()
    {
        optionMenu.HideCanvasGroup();
        controlsMenu.HideCanvasGroup();
        creditsMenu.ShowCanvasGroup();
    }

    #endregion
}
