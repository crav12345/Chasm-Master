using System;
using System.Collections;
using UnityEngine;

public class RiddleSystem : MonoBehaviour
{
    public event Action AskedForRiddle;
    public event Action<ChatGptRiddle> RiddleReceived;
    public event Action<bool> AnswerChecked;

    public bool AwaitingAnswer;
    private ChatGptRiddle _currentRiddle;
    private GameplayInputSystem _inputSystem;
    private bool _gettingRiddle;
    private bool _checkingAnswer;

    public void Initialize(GameplayInputSystem inputSystem)
    {
        AwaitingAnswer = false;

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

        if (AwaitingAnswer)
        {
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
        AwaitingAnswer = false;

        _checkingAnswer = true;

        yield return RiddleUtils.CheckAnswer(_currentRiddle, input, OnCheckedAnswer);
        
        _checkingAnswer = false;
    }

    private void OnCheckedAnswer(bool answerAccepted)
    {
        AnswerChecked?.Invoke(answerAccepted);
    }

    private IEnumerator GetRiddle()
    {
        _gettingRiddle = true;

        AskedForRiddle?.Invoke();
        
        yield return RiddleUtils.GenerateRiddle(_currentRiddle);
        
        _gettingRiddle = false;
        AwaitingAnswer = true;

        Debug.Log(_currentRiddle.Answer);

        RiddleReceived?.Invoke(_currentRiddle);
    }
}
