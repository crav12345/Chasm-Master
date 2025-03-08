using UnityEngine;

public class PaladinSystem : MonoBehaviour
{
    [SerializeField] private GameObject _paladin;
    [SerializeField] private Transform _idleTarget;

    private void Initialize()
    {
        
    }

    private void Update()
    {
        var step = Time.deltaTime * 1.0f;
        _paladin.transform.position = Vector3.MoveTowards(_paladin.transform.position, _idleTarget.position, step);
    }
}
