using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Image npcImage;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private NPCData currentNPC;
    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        if (NPCQueueManager.Instance != null)
        {
            SubscribeToEvents();
        }
    }

    private void OnDisable()
    {
        if (NPCQueueManager.Instance != null)
        {
            UnsubscribeFromEvents();
        }
    }

    private void Start()
    {        
        if (NPCQueueManager.Instance != null)
        {
            SubscribeToEvents();
        }
        
        HideVisual();
    }

    private void HideVisual()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void ShowVisual()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void SubscribeToEvents()
    {
        NPCQueueManager.Instance.OnNPCArrived += HandleNPCArrived;
        NPCQueueManager.Instance.OnNPCServed += HandleNPCServed;
        NPCQueueManager.Instance.OnDayEnded += HandleDayEnded;
    }

    private void UnsubscribeFromEvents()
    {
        NPCQueueManager.Instance.OnNPCArrived -= HandleNPCArrived;
        NPCQueueManager.Instance.OnNPCServed -= HandleNPCServed;
        NPCQueueManager.Instance.OnDayEnded -= HandleDayEnded;
    }

    private void HandleNPCArrived(NPCData npc)
    {
        Debug.Log($"[NPCController] HandleNPCArrived: {npc.npcName}, mask: {npc.requiredMask}");

        currentNPC = npc;
        ShowNPC(npc);

        // Start dialogue based on NPC's required mask
        if (DialogueManager.Instance != null)
        {
            Debug.Log($"[NPCController] Starting dialogue for mask: {npc.requiredMask}");
            DialogueManager.Instance.StartDialogueByMask(npc.requiredMask);
        }
        else
        {
            Debug.LogError("[NPCController] DialogueManager.Instance is NULL!");
        }
    }

    private void HandleNPCServed(NPCData npc, bool wasCorrect)
    {
        // NPC pergi setelah dilayani
        HideNPC();
    }

    private void HandleDayEnded(int day, System.Collections.Generic.List<NPCServeResult> results)
    {
        // NPC hidden di akhir hari
        HideVisual();
    }

    private void ShowNPC(NPCData npc)
    {
        ShowVisual();
        
        if (npcImage != null && npc.npcSprite != null)
        {
            npcImage.sprite = npc.npcSprite;
        }

        // Fade in
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private void HideNPC()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutAndNotify());
    }

    private IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;

        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutAndNotify()
    {
        if (canvasGroup == null)
        {
            // Jika tidak ada CanvasGroup, langsung notify
            NPCQueueManager.Instance?.OnNPCLeft();
            yield break;
        }

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        HideVisual();

        // Notify manager bahwa NPC sudah pergi
        NPCQueueManager.Instance?.OnNPCLeft();
    }
}
