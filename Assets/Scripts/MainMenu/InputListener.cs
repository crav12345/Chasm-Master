using System;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public event Action InputDetected;

    private bool _listen = false;

    public void Initialize()
    {
        _listen = true;
    }

    private void Update()
    {
        if (!_listen)
        {
            return;
        }

        if(Input.anyKeyDown)
        {
            _listen = false;
            InputDetected.Invoke();
        }
    }
}
