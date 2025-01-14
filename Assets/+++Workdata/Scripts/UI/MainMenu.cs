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

    public void StartNewGame()
    {
       GameStateManager.instance.StartNewGame();
    }

    public void OpenMainMenu()
    {
        menu = "Main";
        anim.SetTrigger("Close");
    }

    public void OpenOptionsMenu()
    {
        menu = "Options";
        anim.SetTrigger("Close");
    }

    public void OpenExitMenu()
    {
        menu = "Exit";
        anim.SetTrigger("Close");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

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
