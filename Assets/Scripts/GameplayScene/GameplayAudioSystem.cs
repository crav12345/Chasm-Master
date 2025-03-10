using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the persistent audio source while we're in the gameplay scene.
/// </summary>
public class GameplayAudioSystem : MonoBehaviour
{
    [SerializeField] private AudioClip _gameplayMusic;
    [SerializeField] private AudioClip _menuMusic;

    private AudioSource _audioSource;
    private bool _shouldPlay = false;
    private RiddleSystem _riddleSystem;

    public void Initialize(AudioSource audioSource, RiddleSystem riddleSystem)
    {
        _riddleSystem = riddleSystem;
        _riddleSystem.RiddlePassed += OnRiddlePassed;
        _riddleSystem.RiddleFailed += OnRiddleFailed;

        _audioSource = audioSource;
        _audioSource.clip = _gameplayMusic;
        _audioSource.Play();

        _shouldPlay = true;

        StartCoroutine(AudioUtils.FadeAudio(_audioSource, 0.0f, 1.0f, 3.5f));
    }

    private void OnDestroy()
    {
        _riddleSystem.RiddlePassed -= OnRiddlePassed;
        _riddleSystem.RiddleFailed -= OnRiddleFailed;
    }

    private void Update()
    {
        if (!_shouldPlay)
        {
            return;
        }

        if (_audioSource == null)
        {
            return;
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayDelayed(30f);
        }
    }

    private void OnRiddlePassed()
    {
        StartCoroutine(SwitchToMenuMusic());
    }

    private void OnRiddleFailed()
    {
        StartCoroutine(SwitchToMenuMusic());
    }

    private IEnumerator SwitchToMenuMusic()
    {
        yield return AudioUtils.FadeAudio(_audioSource, 1f, 0f, 3.5f);

        _audioSource.Stop();
        _audioSource.volume = 1f;
        _audioSource.clip = _menuMusic;
    }
}
