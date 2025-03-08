using System.Collections;
using UnityEngine;

public class GameplayAudioSystem : MonoBehaviour
{
    [SerializeField] private AudioClip _songHideDorianConcept;

    private AudioSource _audioSource;

    public void Initialize(AudioSource audioSource)
    {
        _audioSource = audioSource;
        _audioSource.clip = _songHideDorianConcept;
        _audioSource.Play();
        StartCoroutine(AudioUtils.FadeAudio(_audioSource, 0.0f, 1.0f, 3.5f));
    }

    private void Update()
    {
        if (_audioSource == null)
        {
            return;
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayDelayed(30f);
        }
    }
}
