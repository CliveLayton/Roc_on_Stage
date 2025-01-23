using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    #region Variables

    private const string FULLSCREEN_KEY = "FullScreen";

    [SerializeField] private Toggle fullScreenToggle;

    #endregion

    #region Unity Methods

    private void Start()
    {
        GameStateManager.instance.onStateChanged += OnStateChange;
        LoadSettings();
    }

    #endregion

    #region GameSettings Methods

    private void OnStateChange(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.InGame)
        {
            LoadSettings();
        }
    }

    /// <summary>
    /// if playerpref keys are available set settings or set default values
    /// </summary>
    private void LoadSettings()
    {
        fullScreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) > 0;
    }

    /// <summary>
    /// sets fullscreen and saves current settings
    /// </summary>
    /// <param name="isFullscreen"></param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
    }

    #endregion
}
