using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueRegistry", menuName = "Dialogue/Registry")]
public class DialogueRegistry : ScriptableObject
{
    [SerializeField] private DialogueEntry[] dialogues;

    private Dictionary<MaskNeeded, List<DialogueEntry>> dialoguesByMask;

    private void OnEnable()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        dialoguesByMask = new Dictionary<MaskNeeded, List<DialogueEntry>>();

        foreach (var entry in dialogues)
        {
            if (!dialoguesByMask.ContainsKey(entry.maskType))
            {
                dialoguesByMask[entry.maskType] = new List<DialogueEntry>();
            }
            dialoguesByMask[entry.maskType].Add(entry);
        }
    }

    /// Mendapatkan semua dialog dengan mask tertentu
    public DialogueEntry[] GetDialoguesByMask(MaskNeeded mask)
    {
        if (dialoguesByMask == null) BuildDictionary();

        if (dialoguesByMask.TryGetValue(mask, out var list))
        {
            return list.ToArray();
        }
        
        return System.Array.Empty<DialogueEntry>();
    }

    /// Mendapatkan satu dialog secara random berdasarkan mask
    public DialogueEntry GetRandomDialogueByMask(MaskNeeded mask)
    {
        var dialoguesForMask = GetDialoguesByMask(mask);

        if (dialoguesForMask.Length == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, dialoguesForMask.Length);
        return dialoguesForMask[randomIndex];
    }

    /// Mendapatkan dialog berdasarkan ID spesifik
    public DialogueEntry GetDialogueById(string id)
    {
        if (dialoguesByMask == null) BuildDictionary();

        foreach (var entry in dialogues)
        {
            if (entry.dialogueId == id)
            {
                return entry;
            }
        }
        
        return null;
    }
}
