using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private DialogueRegistry dialogueRegistry;

    private Story currentStory;
    private DialogueEntry currentDialogue;

    // Events
    public event Action<DialogueEntry> OnDialogueStarted;
    public event Action OnDialogueEnded;
    public event Action<string, string> OnDialogueLineChanged; // (speakerName, text)

    public bool IsDialogueActive => currentStory != null;
    public bool CanContinue => currentStory != null && currentStory.canContinue;

    public Button GotoCrafting;
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// Memulai dialog berdasarkan mask (random dari pool yang tersedia)    
    public void StartDialogueByMask(MaskNeeded mask)
    {
        Debug.Log($"[DialogueManager] StartDialogueByMask called for: {mask}");

        if (dialogueRegistry == null) return;

        DialogueEntry entry = dialogueRegistry.GetRandomDialogueByMask(mask);

        if (entry == null) return;
        
        StartDialogue(entry);
    }
    
    /// Memulai dialog berdasarkan ID spesifik    
    public void StartDialogueById(string dialogueId)
    {
        DialogueEntry entry = dialogueRegistry.GetDialogueById(dialogueId);

        if (entry == null)
        {
            Debug.LogError($"[DialogueManager] No dialogue found with id: {dialogueId}");
            return;
        }

        StartDialogue(entry);
    }

    private void StartDialogue(DialogueEntry entry)
    {
        if (entry.inkJsonAsset == null)
        {
            Debug.LogError($"[DialogueManager] Ink JSON asset is null for dialogue: {entry.dialogueId}");
            return;
        }

        GotoCrafting.gameObject.SetActive(false);

        Debug.Log($"[DialogueManager] Starting dialogue: {entry.dialogueId}");

        currentDialogue = entry;
        currentStory = new Story(entry.inkJsonAsset.text);

        Debug.Log($"[DialogueManager] Firing OnDialogueStarted event. Listeners: {OnDialogueStarted?.GetInvocationList()?.Length ?? 0}");
        OnDialogueStarted?.Invoke(entry);

        // Langsung tampilkan line pertama
        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (!IsDialogueActive)
        {
            Debug.LogWarning("[DialogueManager] No active dialogue to continue.");
            return;
        }

        if (currentStory.canContinue)
        {            
            string text = currentStory.Continue().Trim();
            string speakerName = currentStory.currentTags[0];

            OnDialogueLineChanged?.Invoke(speakerName, text);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        currentStory = null;
        currentDialogue = null;

        OnDialogueEnded?.Invoke();
        GotoCrafting.gameObject.SetActive(true);
    }

    public void ForceEndDialogue()
    {
        if (IsDialogueActive)
        {
            EndDialogue();
        }
    }
}
