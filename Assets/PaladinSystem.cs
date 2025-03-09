using System.Collections;
using UnityEngine;

public class PaladinSystem : MonoBehaviour
{
    [SerializeField] private Transform _paladin;
    [SerializeField] private Transform _idleTarget;
    [SerializeField] private Transform _successTarget;

    private RiddleSystem _riddleSystem;

    public void Initialize(RiddleSystem riddleSystem)
    {
        _riddleSystem = riddleSystem;
        _riddleSystem.RiddlePassed += OnRiddlePassed;
        _riddleSystem.RiddleFailed += OnRiddleFailed;

        StartCoroutine(MoveToPosition(_idleTarget));
    }

    private void OnDestroy()
    {
        _riddleSystem.RiddlePassed -= OnRiddlePassed;
        _riddleSystem.RiddleFailed -= OnRiddleFailed;
    }

    private void OnRiddlePassed()
    {
        StartCoroutine(MoveToPosition(_successTarget));
    }

    private void OnRiddleFailed()
    {

    }

    private IEnumerator MoveToPosition(Transform target)
    {
        var elapsed = 0f;
        var duration = 2f;
        var startPosition = _paladin.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _paladin.position = Vector3.Lerp(startPosition, target.position, elapsed/duration);
            yield return null;
        }

        _paladin.position = target.position;
    }
}
