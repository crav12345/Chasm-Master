using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayPresenter : MonoBehaviour
{
    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private RiddleSystem _riddleSystem;
    private GameplayInputSystem _inputSystem;

    public void Initialize(GameplayInputSystem inputSystem, RiddleSystem riddleSystem)
    {
        _inputSystem = inputSystem;
        _riddleSystem = riddleSystem;

        _inputSystem.ReturnPressed += OnReturnPressed;
        _riddleSystem.RiddleReceived += OnRiddleReceived;

        StartCoroutine(StartTutorial());
    }

    private void OnDestroy()
    {
        _inputSystem.ReturnPressed -= OnReturnPressed;
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

    private void OnReturnPressed()
    {
        Debug.Log("Return pressed!");
    }
}
