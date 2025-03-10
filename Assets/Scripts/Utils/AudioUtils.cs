using System.Collections;
using UnityEngine;

/// <summary>
/// Static class for reusable audio methods.
/// </summary>
public static class AudioUtils
{
    /// <summary>
    /// Fades an audio source's volume over time.
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="startVol"></param>
    /// <param name="endVol"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator FadeAudio(AudioSource audioSource, float startVol, float endVol, float duration)
    {
        var elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, endVol, elapsed/duration);
            yield return null;
        }

        audioSource.volume = endVol;
    }
}
