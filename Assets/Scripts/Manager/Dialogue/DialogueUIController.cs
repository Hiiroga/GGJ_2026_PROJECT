using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    private PlayerInput playerInput;

    private void OnEnable()
    {
        if (DialogueManager.Instance != null)
        {
            SubscribeToEvents();
        }

        SetupInput();
    }

    private void OnDisable()
    {
        if (DialogueManager.Instance != null)
        {
            UnsubscribeFromEvents();
        }

        CleanupInput();
    }

    private void Start()
    {
        // Subscribe dialogue when ready
        if (DialogueManager.Instance != null)
        {
            SubscribeToEvents();
        }
        HideDialogue();
    }

    private void SetupInput()
    {
        playerInput = new PlayerInput();
        playerInput.InputAction.NextDialogue.performed += OnContinueInput;
        playerInput.Enable();
    }

    private void CleanupInput()
    {
        if (playerInput != null)
        {
            playerInput.InputAction.NextDialogue.performed -= OnContinueInput;
            playerInput.Disable();
            playerInput.Dispose();
            playerInput = null;
        }
    }

    private void SubscribeToEvents()
    {
        DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
        DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        DialogueManager.Instance.OnDialogueLineChanged += HandleDialogueLineChanged;
    }

    private void UnsubscribeFromEvents()
    {
        DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
        DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
        DialogueManager.Instance.OnDialogueLineChanged -= HandleDialogueLineChanged;
    }

    private void OnContinueInput(InputAction.CallbackContext context)
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            DialogueManager.Instance.ContinueDialogue();
        }
    }

    private void HandleDialogueStarted(DialogueEntry entry)
    {
        ShowDialogue();        
    }

    private void HandleDialogueEnded()
    {
        HideDialogue();
    }

    private void HandleDialogueLineChanged(string speakerName, string text)
    {
        speakerNameText.text = speakerName;
        dialogueText.text = text;
    }

    private void ShowDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
    }

    private void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}
