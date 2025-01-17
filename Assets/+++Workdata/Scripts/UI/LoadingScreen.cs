using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject playerLifeBar;
    [SerializeField] private CanvasGroup gameOverMenu;
    private CanvasGroup loadingPanel;

    private void Awake()
    {
        loadingPanel = GetComponent<CanvasGroup>();
    }

    public void HideLoadingScreen()
    {
        PlayerInputManager playerInput = FindObjectOfType<PlayerInputManager>();
        playerInput.enabled = true;
        loadingPanel.HideCanvasGroup();
    }

    public void StopMusic()
    {
        MusicManager.Instance.StopInGameSFX();
    }

    public void StartMusic()
    {
        gameOverMenu.HideCanvasGroup();
        playerLifeBar.SetActive(true);
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.stageRevolving);
    }
}
