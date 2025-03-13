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
    private GameplayPresenter _gameplayPresenter;

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
        var audioSource = _dependencies.Common.AudioSource;
        yield return AudioUtils.FadeAudio(audioSource, 1.0f, 0.0f, 1.5f);

        var loadOp = SceneManager.LoadSceneAsync("GameplayScene", LoadSceneMode.Single);
        yield return loadOp;

        loadOp = SceneManager.LoadSceneAsync("GameplayView", LoadSceneMode.Additive);
        yield return loadOp;

        loadOp = SceneManager.LoadSceneAsync("Environment", LoadSceneMode.Additive);
        yield return loadOp;

        var gameplayScene = GameObject.Find("GameplayScene");
        var riddleSystem = gameplayScene.GetComponent<RiddleSystem>();
        var gameplayAudio = gameplayScene.GetComponent<GameplayAudioSystem>();
        var inputSystem = gameplayScene.GetComponent<GameplayInputSystem>();
        var paladinSystem = gameplayScene.GetComponent<PaladinSystem>();

        _gameplayPresenter = GameObject.Find("GameplayView").GetComponent<GameplayPresenter>();
        _gameplayPresenter.Initialize(riddleSystem);
        _gameplayPresenter.FadedOut += OnFadedOut;

        _dependencies.Common.LoadingOverlay.SetActive(false);

        gameplayAudio.Initialize(audioSource, riddleSystem);
        riddleSystem.Initialize(inputSystem);
        paladinSystem.Initialize(riddleSystem);
    }

    private void OnFadedOut()
    {
        _gameplayPresenter.FadedOut -= OnFadedOut;

        _mainMenuDependencies = new MainMenuAppState.Dependencies
        {
            Common = _dependencies.Common
        };
    }
}