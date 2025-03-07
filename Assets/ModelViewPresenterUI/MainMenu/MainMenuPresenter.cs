using System.Collections;
using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private CanvasGroup _titleCanvasGroup;
    [SerializeField] private CanvasGroup _beginCanvasGroup;
    [SerializeField] private CanvasGroup _backgroundCanvasGroup;

    public void Initialize()
    {
        StartCoroutine(EnableTitle());
        StartCoroutine(EnableBeginPrompt());
        StartCoroutine(FadeBackground());
    }

    private IEnumerator EnableTitle()
    {
        yield return new WaitForSeconds(5);

        _titleCanvasGroup.alpha = 1.0f;
    }

    private IEnumerator EnableBeginPrompt()
    {
        yield return new WaitForSeconds(7);

        while(true)
        {
            yield return PresenterUtils.FadeCanvasGroup(_beginCanvasGroup, 0.0f, 1.0f,  0.6f);
            yield return PresenterUtils.FadeCanvasGroup(_beginCanvasGroup, 1.0f, 0.0f,  0.6f);
        }
    }

    private IEnumerator FadeBackground()
    {
        yield return new WaitForSeconds(10);

        yield return PresenterUtils.FadeCanvasGroup(_backgroundCanvasGroup, 1.0f, 0.0f, 1.0f);
    }
}
