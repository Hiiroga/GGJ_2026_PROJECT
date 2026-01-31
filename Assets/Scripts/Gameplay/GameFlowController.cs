using UnityEngine;

/// <summary>
/// Controller untuk mengatur flow gameplay.
/// Attach ke GameObject di scene gameplay.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool autoStartDayOnSceneLoad = true;
    [SerializeField] private float startDelay = 0.5f;

    private void Start()
    {
        if (autoStartDayOnSceneLoad)
        {
            Invoke(nameof(StartNewDay), startDelay);
        }
    }

    /// <summary>
    /// Memulai hari baru
    /// </summary>
    public void StartNewDay()
    {
        if (NPCQueueManager.Instance == null)
        {
            Debug.LogError("[GameFlowController] NPCQueueManager.Instance is null! Make sure NPCQueueManager exists in the scene.");
            return;
        }

        NPCQueueManager.Instance.StartDay();
    }

    /// <summary>
    /// Subscribe ke event end of day untuk handle transisi ke hari berikutnya
    /// </summary>
    private void OnEnable()
    {
        if (NPCQueueManager.Instance != null)
        {
            NPCQueueManager.Instance.OnDayEnded += HandleDayEnded;
        }
    }

    private void OnDisable()
    {
        if (NPCQueueManager.Instance != null)
        {
            NPCQueueManager.Instance.OnDayEnded -= HandleDayEnded;
        }
    }

    private void HandleDayEnded(int day, System.Collections.Generic.List<NPCServeResult> results)
    {
        Debug.Log($"[GameFlowController] Day {day} ended with {results.Count} NPCs served.");
        // TODO: Show end of day UI / results screen
        // Contoh: EndOfDayUI.Instance.ShowResults(results);
    }
}
