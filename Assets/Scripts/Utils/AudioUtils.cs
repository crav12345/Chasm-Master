using System.Collections;
using UnityEngine;

public static class AudioUtils
{
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
