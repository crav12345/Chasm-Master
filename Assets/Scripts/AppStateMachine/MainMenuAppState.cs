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

    public IEnumerator Enter(AppState source, object data)
    {
        var newDependencies = data as Dependencies;
        if (newDependencies == null)
        {
            Debug.LogError($"Trying to enter MainMenu from {source} with invalid data!");
            yield break;
        }

        _dependencies = newDependencies;

        yield return LoadMainMenu(_dependencies);
    }

    public object Exit(AppState destination)
    {
        _dependencies.Common.LoadingOverlay.SetActive(true);
        
        return new object();
    }

    public AppState TryGetTransition()
    {
        return AppState.MainMenu;
    }

    private IEnumerator LoadMainMenu(Dependencies dependencies)
    {
        yield return new WaitForSeconds(3);

        var audioSource = _dependencies.Common.AudioSource;
        audioSource.Play();

        yield return new WaitForSeconds(2);

        var loadOp = SceneManager.LoadSceneAsync("MainMenuView", LoadSceneMode.Single);
        yield return loadOp;

        var presenter = GameObject.Find("MainMenu").GetComponent<MainMenuPresenter>();
        presenter.Initialize();

        _dependencies.Common.LoadingOverlay.SetActive(false);
    }
}