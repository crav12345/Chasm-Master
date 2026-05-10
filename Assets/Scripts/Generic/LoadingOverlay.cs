using UnityEngine;
using TMPro;

/// <summary>
/// Interface for interacting with the persistent LoadingOverlay prefab.
/// </summary>
public class LoadingOverlay : MonoBehaviour
{
    private const float TIME_BETWEEN_DOTS = 0.5f;

    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private TextMeshProUGUI _loadingLabel;
    [SerializeField] private TextMeshProUGUI _pressPlayLabel;

    private float _elapsed;

    private void Update()
    {
        if (!_loadingLabel.enabled)
        {
            return;
        }

        _elapsed += Time.deltaTime;

        if (_elapsed < TIME_BETWEEN_DOTS)
            return;

        if (!_loadingLabel.text.Contains("..."))
            _loadingLabel.text += ".";
        else
            _loadingLabel.text = "Loading";

        _elapsed = 0.0f;
    }

    public void SetActive(bool active)
    {
        _loadingCanvas.enabled = active;
        _pressPlayLabel.enabled = false;
        _loadingLabel.enabled = true;
        _elapsed = 0.0f;
    }
}