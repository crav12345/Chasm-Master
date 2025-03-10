using System;
using UnityEngine;

/// <summary>
/// Broadcasts user input events.
/// </summary>
public class GameplayInputSystem : MonoBehaviour
{
    public event Action ReturnPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ReturnPressed?.Invoke();
        }
    }
}
