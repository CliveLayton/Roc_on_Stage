using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
   #region Variables

   [SerializeField] private AudioMixer mixer;

   [SerializeField] private string volumeKey;

   private Slider slider;

   #endregion

   #region Unity Methods

   private void Start()
   {
      GameStateManager.instance.onStateChanged += OnStateChange;
      slider = GetComponent<Slider>();
      slider.value = PlayerPrefs.GetFloat(volumeKey, 1f);
   }

   #endregion

   #region Volume Settings Methods

   private void OnStateChange(GameStateManager.GameState newState)
   {
      if (newState == GameStateManager.GameState.InGame)
      {
         slider.value = PlayerPrefs.GetFloat(volumeKey, 1f);
      }
   }

   /// <summary>
   /// sets the mixer group value and saves settings
   /// </summary>
   /// <param name="volume">current volume slider float</param>
   public void SetVolume(float volume)
   {
      volume = slider.value;
      mixer.SetFloat(volumeKey, Mathf.Log10(volume) * 20);
      PlayerPrefs.SetFloat(volumeKey, volume);
   }

   #endregion
}
