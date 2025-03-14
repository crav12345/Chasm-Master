using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// In this game, UI presenters are initialized with a serires of models
// (gameplay systems) which they listen to for events. They use those events
// and the data pssed with them as queues to update the UI.
/// </summary>
public class GameplayPresenter : MonoBehaviour
{
    public event Action FadedOut;

    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Canvas _inputCanvas;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private CanvasGroup _fadeCanvasGroup;

    private RiddleSystem _riddleSystem;

    public void Initialize(RiddleSystem riddleSystem)
    {
        _riddleSystem = riddleSystem;
        _riddleSystem.AskedForRiddle += OnAskedForRiddle;
        _riddleSystem.RiddleReceived += OnRiddleReceived;
        _riddleSystem.PlayerSubmittedAnswer += OnPlayerSubmittedAnswer;
        _riddleSystem.RiddlePassed += OnRiddlePassed;
        _riddleSystem.RiddleFailed += OnRiddleFailed;

        StartCoroutine(StartTutorial());
    }

    private void OnDestroy()
    {
        _riddleSystem.AskedForRiddle -= OnAskedForRiddle;
        _riddleSystem.PlayerSubmittedAnswer -= OnPlayerSubmittedAnswer;
        _riddleSystem.RiddleReceived -= OnRiddleReceived;
        _riddleSystem.RiddlePassed -= OnRiddlePassed;
        _riddleSystem.RiddleFailed -= OnRiddleFailed;
    }

    private IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(3);

        _dialogueCanvas.enabled = true;

        _dialogueText.text = "Halt, adventurer! None shall cross my bridge without proving their wit. Answer me this riddle true, or turn back and rue! Fail, and the chasm shall be your road instead!";
    }

    private void OnAskedForRiddle()
    {
        _dialogueText.text = "Hmmm...";
    }

    private void OnRiddleReceived(GetRiddleResponse chatGptRiddle)
    {
        _dialogueText.text = chatGptRiddle.Riddle;

        StartCoroutine(EnableInputField());
    }

    private IEnumerator EnableInputField()
    {
        yield return new WaitForSeconds(1.5f);

        _inputCanvas.enabled = true;
    }

    private void OnPlayerSubmittedAnswer()
    {
        _dialogueText.text = "Hmmm...";
        _inputCanvas.enabled = false;
        _riddleSystem.SubmitAnswer(_inputField.text);
    }

    private void OnRiddlePassed()
    {
        _dialogueText.text = "That is correct! You've proven your wit adventurer. You may cross my chasm.";

        StartCoroutine(FadeOut(2));
    }

    private IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        yield return PresenterUtils.FadeCanvasGroup(_fadeCanvasGroup, 0f, 1f, 2f);

        FadedOut?.Invoke();
    }

    private void OnRiddleFailed()
    {
        _dialogueText.text = "Wrong! A witless wanderer meets a fitting fate. Farewell, floundering fool!";

        StartCoroutine(FadeOut(5));
    }
}
