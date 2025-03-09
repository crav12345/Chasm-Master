using System;
using System.Collections;
using UnityEngine;

public class RiddleSystem : MonoBehaviour
{
    public event Action AskedForRiddle;
    public event Action<ChatGptRiddle> RiddleReceived;
    public event Action RiddlePassed;
    public event Action RiddleFailed;
    public event Action AnswerSubmitted;

    private bool _awaitingAnswer;
    private ChatGptRiddle _currentRiddle;
    private GameplayInputSystem _inputSystem;
    private bool _gettingRiddle;
    private bool _checkingAnswer;

    public void Initialize(GameplayInputSystem inputSystem)
    {
        _awaitingAnswer = false;

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
            AnswerSubmitted?.Invoke();
            return;
        }

        if (_checkingAnswer)
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
        _awaitingAnswer = false;

        _checkingAnswer = true;

        yield return RiddleUtils.CheckAnswer(_currentRiddle, input, OnCheckedAnswer);
        
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
        _gettingRiddle = true;

        AskedForRiddle?.Invoke();
        
        yield return RiddleUtils.GenerateRiddle(_currentRiddle);
        
        _gettingRiddle = false;
        _awaitingAnswer = true;

        Debug.Log(_currentRiddle.Answer);

        RiddleReceived?.Invoke(_currentRiddle);
    }
}
