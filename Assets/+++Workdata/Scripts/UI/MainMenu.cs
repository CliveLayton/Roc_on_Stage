using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Variables

    [SerializeField] private CanvasGroup mainMenuScreen;
    [SerializeField] private CanvasGroup optionScreen;
    [SerializeField] private CanvasGroup exitScreen;
    [SerializeField] private new GameObject light;

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

    public void OpenMainMenu()
    {
        mainMenuScreen.ShowCanvasGroup();
        optionScreen.HideCanvasGroup();
        exitScreen.HideCanvasGroup();
        light.SetActive(true);
    }

    public void OpenOptionMenu()
    {
        optionScreen.ShowCanvasGroup();
        mainMenuScreen.HideCanvasGroup();
        exitScreen.HideCanvasGroup();
        light.SetActive(false);
    }

    public void OpenExitMenu()
    {
        exitScreen.ShowCanvasGroup();
        mainMenuScreen.HideCanvasGroup();
        optionScreen.HideCanvasGroup();
        light.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
