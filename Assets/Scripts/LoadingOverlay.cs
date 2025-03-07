using UnityEngine;
using TMPro;

public class LoadingOverlay : MonoBehaviour
{
    private const float TIME_BETWEEN_DOTS = 0.5f;

    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private TextMeshProUGUI _loadingLabel;

    private float _elapsed;

    private void Update()
    {
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

        _elapsed = 0.0f;
    }

    public void SetLabelText(string text)
    {
        _loadingLabel.text = text;
    }
}