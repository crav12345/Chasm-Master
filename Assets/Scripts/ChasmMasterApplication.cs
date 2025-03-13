using UnityEngine;

/// <summary>
/// Persistent entry point into the game. Initializes all systems at
/// beginning of runtime. Maintains persistent dependencies.
/// </summary>
public class ChasmMasterApplication : MonoBehaviour
{
    [SerializeField] private AppStateMachine _appStateMachine;
    [SerializeField] private Camera _camera;
    [SerializeField] private LoadingOverlay _loadingOverlay;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Material _skyboxMat;

    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _loadingOverlay.SetActive(true);

        var entryDependencies = new CommonDependencies
        {
            Camera = _camera,
            LoadingOverlay = _loadingOverlay,
            AudioSource = _audioSource,
            SkyboxMat = _skyboxMat,
            ShowMenuFlare = true
        };

        _appStateMachine.Initialize(entryDependencies);
    }
}
