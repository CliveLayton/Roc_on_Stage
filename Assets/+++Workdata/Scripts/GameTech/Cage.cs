using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenCage()
    {
        anim.SetTrigger("Open");
    }

    public void ShowLoadingScreen()
    {
        LoadSceneManager.instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen()
    {
        LoadSceneManager.instance.OpenLoadingScreen();
    }
}
