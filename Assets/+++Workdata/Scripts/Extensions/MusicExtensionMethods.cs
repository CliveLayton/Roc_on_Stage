using System.Collections;
using UnityEngine;

public static class MusicExtensionMethods
{
   /// <summary>
   /// Fades out the current audio clip and fades in the new audio clip
   /// </summary>
   /// <param name="aSource">the audio source</param>
   /// <param name="clip">the clip to play</param>
   /// <param name="duration">the duration to fade</param>
   public static void FadingInOut(this AudioSource aSource, AudioClip clip, float duration)
   {
      aSource.GetComponentInParent<MonoBehaviour>().StartCoroutine(FadeOutAudio(aSource, clip, duration));
   }

   /// <summary>
   /// turn down the volume of the audio source and change the clip after stop playing
   /// goes to fade in coroutine
   /// </summary>
   /// <param name="aSource">the audio source</param>
   /// <param name="clip">the clip to play</param>
   /// <param name="duration">the duration to fade</param>
   /// <returns></returns>
   private static IEnumerator FadeOutAudio(AudioSource aSource, AudioClip clip, float duration)
   {
      while (aSource.volume > 0)
      {
         aSource.volume -= 0.2f * Time.deltaTime / duration;
         yield return null;
      }
      
      aSource.Stop();
      aSource.clip = clip;
      aSource.volume = 1f;
      aSource.GetComponentInParent<MonoBehaviour>().StartCoroutine(FadeInAudio(aSource, duration));
   }

   /// <summary>
   /// plays audio and turn up the volume of the audio source
   /// </summary>
   /// <param name="aSource">the audio source</param>
   /// <param name="duration">the duration to fade</param>
   /// <returns></returns>
   private static IEnumerator FadeInAudio(AudioSource aSource, float duration)
   {
      aSource.volume = 0;
      aSource.Play();

      while (aSource.volume < 1.0f)
      {
         aSource.volume += 0.2f * Time.deltaTime / duration;

         yield return null;
      }

      aSource.volume = 1f;
   }
}
