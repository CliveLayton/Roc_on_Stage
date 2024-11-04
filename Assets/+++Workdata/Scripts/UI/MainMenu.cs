using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Variables

    [SerializeField] private CanvasGroup mainMenuScreen;
    [SerializeField] private CanvasGroup optionScreen;
    [SerializeField] private CanvasGroup exitScreen;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Time.timeScale = 1f;
        mainMenuScreen.ShowCanvasGroup();
        optionScreen.HideCanvasGroup();
        exitScreen.HideCanvasGroup();
    }

    #endregion

    #region Main Menu Methods

    public void StartNewGame()
    {
       GameStateManager.instance.StartNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
