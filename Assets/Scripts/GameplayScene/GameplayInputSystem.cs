using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Broadcasts user input events.
/// </summary>
public class GameplayInputSystem : MonoBehaviour
{
    public event Action ReturnPressed;

    public bool Listening = false;

    public void Initialize()
    {
        StartCoroutine(InitializeCoroutine());
    }

    private IEnumerator InitializeCoroutine()
    {
        yield return new WaitForSeconds(3);

        Listening = true;
    }

    private void Update()
    {
        if (!Listening)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ReturnPressed?.Invoke();
        }
    }
}
