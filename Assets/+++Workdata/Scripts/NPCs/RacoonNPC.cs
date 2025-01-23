using System;
using UnityEngine;

public class RacoonNPC : MonoBehaviour
{
    #region Variables

    private AudioSource audioSource;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    #endregion
    
    #region RacoonNPC Methods

    public void StopHarmonica()
    {
        audioSource.Stop();
    }

    #endregion
}
