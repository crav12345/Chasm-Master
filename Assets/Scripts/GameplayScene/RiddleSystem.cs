using System;
using System.Collections;
using UnityEngine;

public class RiddleSystem : MonoBehaviour
{
    public event Action AskedForRiddle;
    public event Action<ChatGptRiddle> RiddleReceived;

    private ChatGptRiddle _currentRiddle;
    private GameplayInputSystem _inputSystem;
    private bool _gettingRiddle;
    private bool _awaitingAnswer;

    public void Initialize(GameplayInputSystem inputSystem)
    {
        _gettingRiddle = false;
        _awaitingAnswer = false;

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
            return;
        }

        StartCoroutine(GetRiddle());
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
