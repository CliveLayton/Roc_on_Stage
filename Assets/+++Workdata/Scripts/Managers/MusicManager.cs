using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Variables

    public static MusicManager Instance { get; private set; }

    [Header("Audio Sources")] 
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource ambienceAudio;
    [SerializeField] private AudioSource uiSFXAudio;
    [SerializeField] private AudioSource inGameSFXAudio;

    [Header("Music")] 
    public AudioClip mainMenuMusic;

    [Header("Ambience")] 
    public AudioClip theatreAmbience;

    [Header("Player Footsteps")] 
    public AudioClip[] woodSteps = new AudioClip[5];

    [Header("InGame SFX")] 
    public AudioClip openDoor;

    [Header("UI SFX")] 
    public AudioClip buttonHover;
    public AudioClip buttonPress;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Music Methods

    /// <summary>
    /// stops current music and plays the new clip
    /// </summary>
    /// <param name="clip">audio clip to play</param>
    public void PlayMusic(AudioClip clip, float fadeDuration)
    {
        musicAudio.FadingInOut(clip, fadeDuration);
    }

    /// <summary>
    /// stops current ambience and plays the new clip
    /// </summary>
    /// <param name="clip">audio clip to play</param>
    public void PlayAmbience(AudioClip clip, float fadeDuration)
    {
        ambienceAudio.FadingInOut(clip, fadeDuration);
    }

    /// <summary>
    /// plays ui sfx audio clip one time
    /// </summary>
    /// <param name="clip">audio clip to play</param>
    public void PlayUISFX(AudioClip clip)
    {
        uiSFXAudio.PlayOneShot(clip);
    }

    /// <summary>
    /// plays in game sfx audio clip one time
    /// </summary>
    /// <param name="clip">audio clip to play</param>
    public void PlayInGameSFX(AudioClip clip)
    {
        inGameSFXAudio.PlayOneShot(clip);
    }

    #endregion
}
