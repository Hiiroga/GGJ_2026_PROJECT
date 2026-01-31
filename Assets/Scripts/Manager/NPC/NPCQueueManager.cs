using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    public static NPCQueueManager Instance { get; private set; }

    [Header("NPC Pool")]
    [SerializeField] private NPCData[] availableNPCs;

    [Header("Settings")]
    [SerializeField] private int minNPCPerDay = 2;
    [SerializeField] private int maxNPCPerDay = 5;

    private Queue<NPCData> npcQueue = new Queue<NPCData>();
    private List<NPCServeResult> dailyResults = new List<NPCServeResult>();

    // Events
    public event Action<int> OnDayStarted;  // passes day number
    public event Action<int, List<NPCServeResult>> OnDayEnded;  // passes day number and results
    public event Action<NPCData> OnNPCArrived;
    public event Action<NPCData, bool> OnNPCServed;  // passes NPC and whether correct mask was given
    public event Action OnAllNPCsServed;

    // Properties
    public int CurrentDay { get; private set; } = 0;
    public int RemainingNPCs => npcQueue.Count;
    public NPCData CurrentNPC { get; private set; }
    public bool IsNPCWaiting => CurrentNPC != null;

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

    public void StartDay()
    {
        CurrentDay++;
        dailyResults.Clear();
        GenerateNPCQueue();

        OnDayStarted?.Invoke(CurrentDay);

        // Panggil NPC pertama
        CallNextNPC();
    }

    private void GenerateNPCQueue()
    {
        npcQueue.Clear();

        int npcCount = UnityEngine.Random.Range(minNPCPerDay, maxNPCPerDay + 1);

        // Shuffle dan ambil NPC random
        List<NPCData> shuffled = new List<NPCData>(availableNPCs);
        ShuffleList(shuffled);

        for (int i = 0; i < npcCount && i < shuffled.Count; i++)
        {
            npcQueue.Enqueue(shuffled[i]);
        }

        Debug.Log($"[NPCQueueManager] Day {CurrentDay} started with {npcQueue.Count} NPCs");
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void CallNextNPC()
    {
        if (npcQueue.Count > 0)
        {
            CurrentNPC = npcQueue.Dequeue();
            OnNPCArrived?.Invoke(CurrentNPC);
            Debug.Log($"[NPCQueueManager] NPC arrived: {CurrentNPC.npcName}, needs mask: {CurrentNPC.requiredMask}");
        }
        else
        {
            CurrentNPC = null;
            EndDay();
        }
    }
    
    // Serve NPC saat ini (player memberikan item)
    // Panggil waktu mau kasih mask ke NPC
    public void ServeCurrentNPC(MaskNeeded givenMask)
    {
        if (CurrentNPC == null)
        {
            Debug.LogWarning("[NPCQueueManager] No NPC to serve!");
            return;
        }

        bool isCorrect = givenMask == CurrentNPC.requiredMask;

        // Simpan hasil untuk end of day
        dailyResults.Add(new NPCServeResult
        {
            npcData = CurrentNPC,
            givenMask = givenMask,
            wasCorrect = isCorrect
        });

        OnNPCServed?.Invoke(CurrentNPC, isCorrect);
        Debug.Log($"[NPCQueueManager] Served {CurrentNPC.npcName} with {givenMask}. Correct: {isCorrect}");

        // NPC pergi, panggil berikutnya
        CurrentNPC = null;
    }
 
    public void OnNPCLeft()
    {
        if (npcQueue.Count > 0)
        {
            CallNextNPC();
        }
        else
        {
            OnAllNPCsServed?.Invoke();
            EndDay();
        }
    }

    private void EndDay()
    {
        Debug.Log($"[NPCQueueManager] Day {CurrentDay} ended. Results: {dailyResults.Count} NPCs served");
        OnDayEnded?.Invoke(CurrentDay, new List<NPCServeResult>(dailyResults));
    }
    
    /// Mendapatkan skor hari ini 
    public int GetTodayScore()
    {
        int score = 0;
        foreach (var result in dailyResults)
        {
            if (result.wasCorrect) score++;
        }
        return score;
    }
    
    // Mendapatkan hasil lengkap hari ini
    public List<NPCServeResult> GetTodayResults()
    {
        return new List<NPCServeResult>(dailyResults);
    }
}
