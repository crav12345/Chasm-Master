using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Progression system responsible for broadcasting events about the riddle and
// results.
/// </summary>
public class RiddleSystem : MonoBehaviour
{
    public event Action AskedForRiddle;
    public event Action<GetRiddleResponse> RiddleReceived;
    public event Action RiddlePassed;
    public event Action RiddleFailed;
    public event Action PlayerSubmittedAnswer;

    private GetRiddleResponse _currentRiddle;
    private GameplayInputSystem _inputSystem;
    private bool _awaitingAnswer;
    private bool _gettingRiddle;
    private bool _checkingAnswer;
    private bool _gotRiddle;

    public void Initialize(GameplayInputSystem inputSystem)
    {
        _awaitingAnswer = false;
        _gotRiddle = false;
        _gettingRiddle = false;
        _checkingAnswer = false;

        _currentRiddle = new();

        _inputSystem = inputSystem;
        _inputSystem.ReturnPressed += OnReturnPressed;
    }

    private void OnReturnPressed()
    {
        if (_gettingRiddle)
        {
            return;
        }

        if (_awaitingAnswer)
        {
            PlayerSubmittedAnswer?.Invoke();
            return;
        }

        if (_checkingAnswer)
        {
            return;
        }

        if (_gotRiddle)
        {
            return;
        }

        StartCoroutine(GetRiddle());
    }

    public void SubmitAnswer(string input)
    {
        if (_checkingAnswer)
        {
            return;
        }

        StartCoroutine(CheckAnswer(input));
    }

    private IEnumerator CheckAnswer(string input)
    {
        _inputSystem.Listening = false;

        _awaitingAnswer = false;

        _checkingAnswer = true;

        yield return new WaitForSeconds(1);
        yield return RiddleUtils.CheckAnswer(_currentRiddle.RiddleId, input, OnCheckedAnswer);
        
        _checkingAnswer = false;
    }

    private void OnCheckedAnswer(bool answerAccepted)
    {
        if (answerAccepted)
        {
            RiddlePassed?.Invoke();
            return;
        }

        RiddleFailed?.Invoke();
    }

    private IEnumerator GetRiddle()
    {
        _inputSystem.Listening = false;
        _gotRiddle = true;
        _gettingRiddle = true;

        AskedForRiddle?.Invoke();
        
        yield return RiddleUtils.GetRiddle(GetRiddleCallback);
    }

    private void GetRiddleCallback(GetRiddleResponse response)
    {
        _currentRiddle = response;
        _gettingRiddle = false;
        _awaitingAnswer = true;

        StartCoroutine(EnableListenerAfterDelay(3.3f));

        RiddleReceived?.Invoke(_currentRiddle);
    }

    private IEnumerator EnableListenerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _inputSystem.Listening = true;
    }
}
