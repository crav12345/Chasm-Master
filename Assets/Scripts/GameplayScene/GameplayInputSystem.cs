using System;
using UnityEngine;

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
