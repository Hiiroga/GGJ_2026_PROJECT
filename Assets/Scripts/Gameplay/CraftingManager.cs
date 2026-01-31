using DG.Tweening;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [Header("Panel")]
    public RectTransform panelCrafting;
    public CanvasGroup canvasGroup;

    [Header("Animation")]
    public float duration = 0.4f;
    public Ease openEase = Ease.OutCubic;
    public Ease closeEase = Ease.InCubic;

    Vector2 onScreenPos;
    Vector2 offScreenPos;

    bool isAnimating;

    void Awake()
    {
        Instance = this;

        onScreenPos = panelCrafting.anchoredPosition;
        offScreenPos = onScreenPos + new Vector2(0, -Screen.height);

        panelCrafting.anchoredPosition = offScreenPos;
        canvasGroup.alpha = 0f;

        panelCrafting.gameObject.SetActive(false);
    }

    // ================= OPEN =================

    public void OpenCrafting()
    {
        if (isAnimating) return;

        isAnimating = true;
        panelCrafting.gameObject.SetActive(true);

        panelCrafting.anchoredPosition = offScreenPos;
        canvasGroup.alpha = 0f;

        DOTween.Sequence()
            .Append(panelCrafting.DOAnchorPos(onScreenPos, duration).SetEase(openEase))
            .Join(canvasGroup.DOFade(1f, duration * 0.8f))
            .OnComplete(() =>
            {
                isAnimating = false;
            });
    }

    // ================= CLOSE =================

    public void CloseCrafting()
    {
        if (isAnimating || !panelCrafting.gameObject.activeSelf) return;

        isAnimating = true;

        DOTween.Sequence()
            .Append(panelCrafting.DOAnchorPos(offScreenPos, duration).SetEase(closeEase))
            .Join(canvasGroup.DOFade(0f, duration * 0.8f))
            .OnComplete(() =>
            {
                panelCrafting.gameObject.SetActive(false);
                isAnimating = false;
            });
    }
}
