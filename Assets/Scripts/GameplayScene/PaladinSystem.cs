using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PaladinSystem : MonoBehaviour
{
    [SerializeField] private Transform _paladin;
    [SerializeField] private Transform _idleTarget;
    [SerializeField] private Transform _successTarget;
    [SerializeField] private Animator _animator;

    private RiddleSystem _riddleSystem;

    public void Initialize(RiddleSystem riddleSystem)
    {
        _riddleSystem = riddleSystem;
        _riddleSystem.RiddlePassed += OnRiddlePassed;
        _riddleSystem.RiddleFailed += OnRiddleFailed;

        StartCoroutine(MoveToPosition(_idleTarget, 2f));
    }

    private void OnDestroy()
    {
        _riddleSystem.RiddlePassed -= OnRiddlePassed;
        _riddleSystem.RiddleFailed -= OnRiddleFailed;
    }

    private void OnRiddlePassed()
    {
        StartCoroutine(RotateTowards(_successTarget, 0.5f));
        StartCoroutine(MoveToPosition(_successTarget, 5f));
    }

    private void OnRiddleFailed()
    {

    }

    private IEnumerator RotateTowards(Transform target, float duration)
    {
        var elapsed = 0f;
        var startRotation = _paladin.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            _paladin.rotation = Quaternion.Slerp(startRotation, target.rotation, elapsed/duration);

            yield return null;
        }

        _paladin.rotation = target.rotation;
    }

    private IEnumerator MoveToPosition(Transform target, float duration)
    {
        _animator.SetBool("Walking", true);

        var elapsed = 0f;
        var startPosition = _paladin.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            _paladin.position = Vector3.Lerp(startPosition, target.position, elapsed/duration);
            
            yield return null;
        }

        _paladin.position = target.position;
        
        _animator.SetBool("Walking", false);
    }
}
