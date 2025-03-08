using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayPresenter : MonoBehaviour
{
    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Canvas _inputCanvas;

    private RiddleSystem _riddleSystem;

    public void Initialize(RiddleSystem riddleSystem)
    {
        _riddleSystem = riddleSystem;
        _riddleSystem.RiddleReceived += OnRiddleReceived;
        _riddleSystem.AskedForRiddle += OnAskedForRiddle;

        StartCoroutine(StartTutorial());
    }

    private void OnDestroy()
    {
        _riddleSystem.RiddleReceived -= OnRiddleReceived;
        _riddleSystem.AskedForRiddle -= OnAskedForRiddle;
    }

    private IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(3);

        _dialogueCanvas.enabled = true;

        _dialogueText.text = "Halt, adventurer! None shall cross my bridge without proving their wit. Answer me this riddle true, or turn back and rue! Fail, and the chasm shall be your road instead!";
    }

    private void OnAskedForRiddle()
    {
        _dialogueText.text = "Hmmm...";
    }

    private void OnRiddleReceived(ChatGptRiddle chatGptRiddle)
    {
        _dialogueText.text = chatGptRiddle.Riddle;

        StartCoroutine(EnableInputField());
    }

    private IEnumerator EnableInputField()
    {
        yield return new WaitForSeconds(1.5f);

        _inputCanvas.enabled = true;
    }
}
