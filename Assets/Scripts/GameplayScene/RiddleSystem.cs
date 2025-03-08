using System;
using System.Collections;
using UnityEngine;

public class RiddleSystem : MonoBehaviour
{
    public event Action RiddleReceived;

    private ChatGptRiddle _currentRiddle;

    public void Initialize()
    {
        StartCoroutine(GetRiddle());        
    }

    private IEnumerator GetRiddle()
    {
        yield return RiddleUtils.GenerateRiddle(_currentRiddle);

        RiddleReceived?.Invoke();
    }
}
