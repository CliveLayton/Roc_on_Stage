using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Variables

    [SerializeField] private CanvasGroup mainMenuScreen;
    [SerializeField] private CanvasGroup optionScreen;
    [SerializeField] private CanvasGroup exitScreen;

    private Animator anim;
    private string menu;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Time.timeScale = 1f;
        mainMenuScreen.ShowCanvasGroup();
        optionScreen.HideCanvasGroup();
        exitScreen.HideCanvasGroup();
        anim = GetComponent<Animator>();
    }

    #endregion

    #region Main Menu Methods

    /// <summary>
    /// load the story scene and lock the cursor
    /// </summary>
    public void StartNewGame()
    {
       LoadSceneManager.instance.SwitchScene(GameStateManager.storySceneName,false);
       Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// opens the main menu and activate the trigger to close curtain
    /// </summary>
    public void OpenMainMenu()
    {
        menu = "Main";
        anim.SetTrigger("Close");
    }

    /// <summary>
    /// opens the options menu and activate the trigger to close curtain
    /// </summary>
    public void OpenOptionsMenu()
    {
        menu = "Options";
        anim.SetTrigger("Close");
    }

    /// <summary>
    /// opens the exit menu and activate the trigger to close curtain
    /// </summary>
    public void OpenExitMenu()
    {
        menu = "Exit";
        anim.SetTrigger("Close");
    }

    /// <summary>
    /// quit the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// activate the current menu and trigger the open animation for the curtain
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitToOpenCurtain()
    {
        switch (menu)
        {
            case "Main":
                mainMenuScreen.ShowCanvasGroup();
                optionScreen.HideCanvasGroup();
                exitScreen.HideCanvasGroup();
                break;
            case "Options":
                optionScreen.ShowCanvasGroup();
                mainMenuScreen.HideCanvasGroup();
                exitScreen.HideCanvasGroup();
                break;
            case "Exit":
                exitScreen.ShowCanvasGroup();
                mainMenuScreen.HideCanvasGroup();
                optionScreen.HideCanvasGroup();
                break;
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Open");
    }

    #endregion
}
