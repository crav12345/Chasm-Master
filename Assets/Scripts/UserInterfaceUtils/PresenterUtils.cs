using System.Collections;
using UnityEngine;

public static class PresenterUtils
{
    /// <summary>
    /// Fades a canvas group over a specified duration in a coroutine.
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="startAlpha"></param>
    /// <param name="endAlpha"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        var elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
