using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AppStateMachine;

/// <summary>
/// Represents the initial gameplay state in the App state machine.
/// </summary>
public class GameplayAppState : IAppState
{
    public class Dependencies
    {
        public CommonDependencies Common;
    }

    private Dependencies _dependencies;
    private MainMenuAppState.Dependencies _mainMenuDependencies;

    public IEnumerator Enter(AppState source, object data)
    {
        var newDependencies = data as Dependencies;
        if (newDependencies == null)
        {
            Debug.LogError($"Trying to enter Title from {source} with invalid data!");
            yield break;
        }

        _dependencies = newDependencies;
        _mainMenuDependencies = null;

        yield return LoadMainMenu(_dependencies);
    }

    public object Exit(AppState destination)
    {
        _dependencies.Common.LoadingOverlay.SetActive(true);

        if (destination == AppState.MainMenu)
            return _mainMenuDependencies;

        return new object();
    }

    public AppState TryGetTransition()
    {
        if (_mainMenuDependencies != null)
            return AppState.MainMenu;

        return AppState.Gameplay;
    }

    private IEnumerator LoadMainMenu(Dependencies dependencies)
    {
        // TODO: Fade out.
        var audioSource = _dependencies.Common.AudioSource;
        audioSource.Stop();

        yield return new WaitForSeconds(2);

        var loadOp = SceneManager.LoadSceneAsync("Environment", LoadSceneMode.Single);
        yield return loadOp;

        _dependencies.Common.LoadingOverlay.SetActive(false);
    }

    private void OnGameQuit()
    {
        _mainMenuDependencies = new MainMenuAppState.Dependencies
        {
            Common = _dependencies.Common
        };
    }
}