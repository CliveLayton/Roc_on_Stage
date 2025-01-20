using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    #region Variables

    [SerializeField] private PlayerInputManager playerInput;
    [SerializeField] private Animator faboCage;
    private Animator UiAnim;
    

    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerInput.enabled = false;
        UiAnim = GetComponent<Animator>();
    }

    #endregion

    #region EndingScene Methods

    public void StartScene()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void RescueFabo()
    {
        UiAnim.SetTrigger("Rescue");
    }

    public void OpenCage()
    {
        faboCage.SetTrigger("Open");
    }

    public void LeaveFabo()
    {
        UiAnim.SetTrigger("FadeOut");
    }

    public void LoadCreditScene()
    {
        GameStateManager.instance.LoadNewGameplayScene(GameStateManager.creditSceneName);
    }

    public void ShowLoadingScreen()
    {
        LoadSceneManager.instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen()
    {
        LoadSceneManager.instance.OpenLoadingScreen();
    }

    #endregion
}
