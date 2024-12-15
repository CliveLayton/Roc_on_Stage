using System;
using UnityEngine;

/// <summary>
/// The GameStateManager lies at the heart of our code.
/// Most importantly for this demonstration, it contains the GameData
/// and manages the loading and saving of our save files.
/// Additionally, it manages the loading and unloading of levels, as well as going back to the main menu.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    #region Variables

    public static GameStateManager instance;

    public const string mainMenuSceneName = "Main Menu";
    public const string level1SceneName = "Forest";
    public const string level2SceneName = "Town";
    public const string level3SceneName = "Castle";
    public const string level4SceneName = "Boss";

    public enum GameState
    {
        InMainMenu = 0,
        InGame = 1
    }

    //this event notifies any objects that need to know about the changing of the game state.
    public event Action<GameState> onStateChanged;
    
    //the current state
    public GameState currentState { get; private set; } = GameState.InMainMenu;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //when we start the game, we first want to enter the main menu
        GoToMainMenu();
    }

    #endregion

    #region GameState Manager Methods

    /// <summary>
    /// called to enter the main menu. Also changes the game state
    /// </summary>
    /// <param name="showLoadingScreen">with or without loading screen</param>
    public void GoToMainMenu(bool showLoadingScreen = true)
    {
        currentState = GameState.InMainMenu;
        if (onStateChanged != null)
        {
            onStateChanged(currentState);
        }
        LoadSceneManager.instance.SwitchScene(mainMenuSceneName,showLoadingScreen);
        //MusicManager.Instance.PlayMusic(MusicManager.Instance.mainMenuMusic, 0.1f);
    }

    //called to start a new game. Also changes the game state.
    public void StartNewGame()
    {
        currentState = GameState.InGame;
        if (onStateChanged != null)
        {
            onStateChanged(currentState);
        }
        LoadSceneManager.instance.SwitchScene(level1SceneName);
    }

    public void LoadNewGameplayScene(string sceneName)
    {
        if (currentState == GameState.InMainMenu)
        {
            return;
        }
        
        LoadSceneManager.instance.SwitchScene(sceneName);
    }

    #endregion
}
