using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] private Animator npcAnim;
    private PlayerInputManager playerInput;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = FindObjectOfType<PlayerInputManager>();
    }

    public void OpenCage()
    {
        anim.SetTrigger("Open");
    }

    public void ShowLoadingScreen()
    {
        playerInput.enabled = false;
        LoadSceneManager.instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen()
    {
        playerInput.enabled = true;
        LoadSceneManager.instance.OpenLoadingScreen();
    }

    public void HappyNpc()
    {
        GameStateManager.instance.npcCounter += 1;
        npcAnim.SetTrigger("Happy");
    }
}
