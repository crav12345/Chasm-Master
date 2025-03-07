using System.Collections;
using UnityEngine;
using static AppStateMachine;

/// <summary>
/// Represents common applications dependencies used across many states
/// </summary>
public class CommonDependencies
{
    public Camera Camera;
    public LoadingOverlay LoadingOverlay;
    public AudioSource AudioSource;
}

/// <summary>
/// Represents the application's entry state in the App state machine.
/// </summary>
public class EntryAppState : IAppState
{
    private CommonDependencies _dependencies;
    public EntryAppState(CommonDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public IEnumerator Enter(AppState source, object data)
    {
        yield break;
    }

    public object Exit(AppState destination)
    {
        if (destination == AppState.MainMenu)
        {
            return new MainMenuAppState.Dependencies()
            {
                Common = _dependencies
            };
        }

        return new object();
    }

    public AppState TryGetTransition()
    {
        return AppState.MainMenu;
    }
}