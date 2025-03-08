using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayPresenter : MonoBehaviour
{
    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Canvas _inputCanvas;
    [SerializeField] private TMP_InputField _inputField;

    private RiddleSystem _riddleSystem;
    private GameplayInputSystem _inputSystem;

    public void Initialize(RiddleSystem riddleSystem, GameplayInputSystem inputSystem)
    {
        _inputSystem = inputSystem;
        _inputSystem.ReturnPressed += OnReturnPressed;

        _riddleSystem = riddleSystem;
        _riddleSystem.AskedForRiddle += OnAskedForRiddle;
        _riddleSystem.RiddleReceived += OnRiddleReceived;
        _riddleSystem.AnswerChecked += OnAnswerChecked;

        StartCoroutine(StartTutorial());
    }

    private void OnDestroy()
    {
        _inputSystem.ReturnPressed -= OnReturnPressed;
        _riddleSystem.AskedForRiddle -= OnAskedForRiddle;
        _riddleSystem.RiddleReceived -= OnRiddleReceived;
        _riddleSystem.AnswerChecked -= OnAnswerChecked;
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

    private void OnReturnPressed()
    {
        if (!_riddleSystem.AwaitingAnswer)
        {
            return;
        }

        _inputCanvas.enabled = false;
        _riddleSystem.SubmitAnswer(_inputField.text);
    }

    private void OnAnswerChecked(bool answerAccepted)
    {
        if (answerAccepted)
        {
            _dialogueText.text = "That is correct! You've proven your wit adventurer. You may cross my chasm.";
            return;
        }

        _dialogueText.text = "Incorrect!";
    }
}
