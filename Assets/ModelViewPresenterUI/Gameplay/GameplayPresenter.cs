using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayPresenter : MonoBehaviour
{
    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private RiddleSystem _riddleSystem;

    public void Initialize(RiddleSystem riddleSystem)
    {
        _riddleSystem = riddleSystem;

        _riddleSystem.RiddleReceived += OnRiddleReceived;

        StartCoroutine(StartTutorial());
    }

    private void OnDestroy()
    {
        _riddleSystem.RiddleReceived -= OnRiddleReceived;
    }

    private IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(3);

        _dialogueCanvas.enabled = true;

        _dialogueText.text = "Halt, adventurer! None shall cross my bridge without proving their wit. Answer me this riddle true, or turn back and rue! Fail, and the chasm shall be your road instead!";
    }

    private void OnRiddleReceived()
    {

    }
}
