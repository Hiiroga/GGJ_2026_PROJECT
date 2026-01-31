using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// Controller untuk mengatur flow gameplay.
/// Attach ke GameObject di scene gameplay.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }
    [Header("Settings")]
    [SerializeField] private bool autoStartDayOnSceneLoad = true;
    [SerializeField] private float startDelay = 0.5f;

    public Button NextDay;

    private void Awake()
    {
        Instance = this;
    }

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
        int score = 0;
        int ScoreAll;
        foreach (var result in results)
        {
            if (!result.wasCorrect) score++;
        }
        ScoreAll = PlayerPrefs.GetInt("Score") + score;
        PlayerPrefs.SetInt("Score", ScoreAll);
        Debug.Log($"[GameFlowController] Day {day} ended with {score} Wrong");
        Debug.Log($"PlayerPrefs = {ScoreAll}");
        // TODO: Show end of day UI / results screen
        EndOfDayUI.Instance.Results = score;
        NextDay.gameObject.SetActive(true);
    }
}
