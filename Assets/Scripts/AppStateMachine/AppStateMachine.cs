using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages IAppState implementations and transitions between them.
/// </summary>
public class AppStateMachine : MonoBehaviour
{
    /// <summary>
    /// Interface for loading assets and controlling navigation in the game.
    /// </summary>
    public interface IAppState
    {
        IEnumerator Enter(AppState source, object data);

        object Exit(AppState destination);

        AppState TryGetTransition();
    }

    public enum AppState
    {
        Entry,
        MainMenu,
        Gameplay,
    }

    private AppState _currentState = AppState.Entry;
    private Dictionary<AppState, IAppState> _stateMap;

    public void Initialize(CommonDependencies entryDependencies)
    {
        _stateMap = new Dictionary<AppState, IAppState>
        {
            { AppState.Entry, new EntryAppState(entryDependencies) },
            { AppState.MainMenu, new MainMenuAppState() },
            { AppState.Gameplay, new GameplayAppState() },
        };
    }

    void Update()
    {
        if (_stateMap == null)
        {
            return;
        }

        if (!_stateMap.ContainsKey(_currentState))
        {
            return;
        }
        var cur = _stateMap[_currentState];
        var nextState = cur.TryGetTransition();

        if (nextState == _currentState)
        {
            return;
        }

        if (!_stateMap.ContainsKey(nextState))
        {
            return;
        }

        object data = cur.Exit(nextState);
        StartCoroutine(EnterNextState(nextState, data));
    }

    private IEnumerator EnterNextState(AppState nextState, object data)
    {
        var sourceState = _currentState;
        _currentState = nextState;

        yield return _stateMap[nextState].Enter(sourceState, data);
    }
}