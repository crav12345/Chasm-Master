using System;
using System.Collections;
using UnityEngine;

public class PaladinSystem : MonoBehaviour
{
    public event Action<bool> ToggledRagdoll;

    [SerializeField] private Transform _paladinTransform;
    [SerializeField] private Transform _idleTarget;
    [SerializeField] private Transform _successTarget;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _throwTarget;

    private Rigidbody[] _rigidBodies;

    private RiddleSystem _riddleSystem;

    private void Awake()
    {
        _rigidBodies = _paladinTransform.GetComponentsInChildren<Rigidbody>();
        ToggleRagdoll(false);
    }
    
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
        ToggleRagdoll(true);

        var throwForce = 250f;
        var throwDir = _throwTarget.position - _paladinTransform.position;

        foreach (var rb in _rigidBodies)
        {
            rb.AddForce(throwDir.normalized * throwForce, ForceMode.Impulse);
        }
    }

    private IEnumerator RotateTowards(Transform target, float duration)
    {
        var elapsed = 0f;
        var startRotation = _paladinTransform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            _paladinTransform.rotation = Quaternion.Slerp(startRotation, target.rotation, elapsed/duration);

            yield return null;
        }

        _paladinTransform.rotation = target.rotation;
    }

    private IEnumerator MoveToPosition(Transform target, float duration)
    {
        _animator.SetBool("Walking", true);

        var elapsed = 0f;
        var startPosition = _paladinTransform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            _paladinTransform.position = Vector3.Lerp(startPosition, target.position, elapsed/duration);
            
            yield return null;
        }

        _paladinTransform.position = target.position;
        
        _animator.SetBool("Walking", false);
    }

    private void ToggleRagdoll(bool enabled)
    {
        foreach (var rigidBody in _rigidBodies)
        {
            rigidBody.isKinematic = !enabled;
        }

        _animator.enabled = !enabled;

        ToggledRagdoll?.Invoke(enabled);
    }
}
