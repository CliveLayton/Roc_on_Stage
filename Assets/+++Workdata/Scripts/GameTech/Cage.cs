using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] private Animator npcAnim;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenCage()
    {
        anim.SetTrigger("Open");
        GameStateManager.instance.npcCounter += 1;
    }

    public void ShowLoadingScreen()
    {
        LoadSceneManager.instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen()
    {
        LoadSceneManager.instance.OpenLoadingScreen();
    }

    public void HappyNpc()
    {
        npcAnim.SetTrigger("Happy");
    }
}
