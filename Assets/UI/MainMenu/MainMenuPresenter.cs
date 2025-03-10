using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// In this game, UI presenters are initialized with a serires of models
// (gameplay systems) which they listen to for events. They use those events
// and the data pssed with them as queues to update the UI.
/// </summary>
public class MainMenuPresenter : MonoBehaviour
{
    public event Action MainMenuClosed;

    [SerializeField] private CanvasGroup _titleCanvasGroup;
    [SerializeField] private CanvasGroup _beginCanvasGroup;
    [SerializeField] private CanvasGroup _backgroundCanvasGroup;

    private InputListener _inputListener;
    private bool _flashPrompt;

    public void Initialize(InputListener inputListener)
    {
        _inputListener = inputListener;
        _inputListener.InputDetected += OnInputDetected;

        _flashPrompt = true;

        StartCoroutine(EnableTitle());
        StartCoroutine(EnableBeginPrompt());
        StartCoroutine(FadeBackground());
    }

    private void OnDestroy()
    {
        _inputListener.InputDetected -= OnInputDetected;   
    }

    private IEnumerator EnableTitle()
    {
        yield return new WaitForSeconds(5);

        _titleCanvasGroup.alpha = 1.0f;
    }

    private IEnumerator EnableBeginPrompt()
    {
        yield return new WaitForSeconds(7);

        while(_flashPrompt)
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

    private void OnInputDetected()
    {
        StopAllCoroutines();
        StartCoroutine(CloseMainMenu());
    }

    private IEnumerator CloseMainMenu()
    {
        _flashPrompt = false;
        _beginCanvasGroup.alpha = 0.0f;
        _beginCanvasGroup.GetComponentInParent<Canvas>().enabled = false;
        
        var startAlpha = _backgroundCanvasGroup.alpha;
        var endAlpha = 1f;
        var duration = 1f - startAlpha / endAlpha;

        yield return PresenterUtils.FadeCanvasGroup(_backgroundCanvasGroup, startAlpha, endAlpha, duration);

        yield return new WaitForSeconds(1);

        MainMenuClosed.Invoke();
    }
}
