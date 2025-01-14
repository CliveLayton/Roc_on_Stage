using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private CanvasGroup loadingPanel;

    private void Awake()
    {
        loadingPanel = GetComponent<CanvasGroup>();
    }

    public void HideLoadingScreen()
    {
        loadingPanel.HideCanvasGroup();
    }

    public void StopMusic()
    {
        MusicManager.Instance.StopInGameSFX();
    }

    public void StartMusic()
    {
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.stageRevolving);
    }
}
