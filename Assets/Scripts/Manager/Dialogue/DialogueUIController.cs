using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Typewriter Settings")]
    [SerializeField] private float typeSpeed = 0.03f;

    private PlayerInput playerInput;
    private Coroutine typewriterCoroutine;
    private string currentFullText;
    private bool isTyping;

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
        if (Time.timeScale == 0f) return;

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            // If currently typing, complete the text instantly
            if (isTyping)
            {
                CompleteTyping();
            }
            else
            {
                DialogueManager.Instance.ContinueDialogue();
            }
        }
    }

    private void HandleDialogueStarted(DialogueEntry entry)
    {
        ShowDialogue();
    }

    private void HandleDialogueEnded()
    {
        StopTypewriter();
        HideDialogue();
    }

    private void HandleDialogueLineChanged(string speakerName, string text)
    {
        speakerNameText.text = speakerName;
        StartTypewriter(text);
    }

    private void StartTypewriter(string text)
    {
        StopTypewriter();
        currentFullText = text;
        typewriterCoroutine = StartCoroutine(TypewriterRoutine(text));
    }

    private void StopTypewriter()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }
        isTyping = false;
    }

    private void CompleteTyping()
    {
        StopTypewriter();
        dialogueText.text = currentFullText;
    }

    private IEnumerator TypewriterRoutine(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            SfxManager.Instance.Play("textsound");
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        isTyping = false;
        typewriterCoroutine = null;
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
