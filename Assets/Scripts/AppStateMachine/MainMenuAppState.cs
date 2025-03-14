using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using static AppStateMachine;

/// <summary>
/// Represents the application's main menu state in the App state machine.
/// </summary>
public class MainMenuAppState : IAppState
{
    public class Dependencies
    {
        public CommonDependencies Common;
    }

    private Dependencies _dependencies;
    private GameplayAppState.Dependencies _gameplayDependencies;
    private MainMenuPresenter _presenter;

    public IEnumerator Enter(AppState source, object data)
    {
        var newDependencies = data as Dependencies;
        if (newDependencies == null)
        {
            Debug.LogError($"Trying to enter MainMenu from {source} with invalid data!");
            yield break;
        }

        _dependencies = newDependencies;
        _gameplayDependencies = null;

        yield return LoadMainMenu(_dependencies);
    }

    public object Exit(AppState destination)
    {
        _dependencies.Common.LoadingOverlay.SetActive(true);
        
        if (destination == AppState.Gameplay)
        {
            return _gameplayDependencies;
        }

        return new object();
    }

    public AppState TryGetTransition()
    {
        if (_gameplayDependencies != null)
        {
            return AppState.Gameplay;
        }

        return AppState.MainMenu;
    }

    private IEnumerator LoadMainMenu(Dependencies dependencies)
    {   
        // TODO: Probably a better way to do this than all the if-statements.
        var showFlare = _dependencies.Common.ShowMenuFlare;

        if (showFlare)
        {
            yield return new WaitForSeconds(3);
        }

        var audioSource = _dependencies.Common.AudioSource;
        audioSource.Play();
        yield return WaitForAudio(audioSource.clip);

        if (showFlare)
        {
            yield return new WaitForSeconds(2);
        }

        var loadOp = SceneManager.LoadSceneAsync("MainMenuView", LoadSceneMode.Single);
        yield return loadOp;

        loadOp = SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Additive);
        yield return loadOp;

        loadOp = SceneManager.LoadSceneAsync("Environment", LoadSceneMode.Additive);
        yield return loadOp;
        
        var inputListener = GameObject.Find("MainMenuScene").GetComponent<InputListener>();
        _presenter = GameObject.Find("MainMenu").GetComponent<MainMenuPresenter>();
        _presenter.MainMenuClosed += OnMainMenuClosed;
        _presenter.Initialize(inputListener, showFlare);

        RenderSettings.skybox = _dependencies.Common.SkyboxMat;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 35;
        RenderSettings.fogEndDistance = 350;

        _dependencies.Common.LoadingOverlay.SetActive(false);

        if (showFlare)
        {
            yield return new WaitForSeconds(7);
        }

        inputListener.Initialize();
        
        _dependencies.Common.ShowMenuFlare = false;
    }

    private void DeregisterListeners()
    {
        _presenter.MainMenuClosed -= OnMainMenuClosed;
    }

    private void OnMainMenuClosed()
    {
        DeregisterListeners();
        
        _gameplayDependencies = new GameplayAppState.Dependencies
        {
            Common = _dependencies.Common
        };
    }

    private IEnumerator WaitForAudio(AudioClip clip)
    {
        while (clip.loadState != AudioDataLoadState.Loaded)
        {
            yield return null;
        }
    }
}