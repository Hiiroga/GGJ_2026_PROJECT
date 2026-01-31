using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class CreditManager : MonoBehaviour
{
    public TMP_Text creditText;
    public RectTransform creditRect;

    [Header("Scroll Settings")]
    public float scrollDuration = 20f;
    public float startYOffset = -600f;
    public float endYOffset = 800f;

    [Header("Fade")]
    public float fadeDuration = 1f;

    [Header("Skip")]
    public bool allowSkip = true;

    Vector2 startPos;
    Vector2 endPos;
    Tween scrollTween;

    void Start()
    {
        startPos = new Vector2(creditRect.anchoredPosition.x, startYOffset);
        endPos = new Vector2(creditRect.anchoredPosition.x, endYOffset);

        creditRect.anchoredPosition = startPos;

        CanvasGroup cg = GetOrAddCanvasGroup(creditText);
        cg.alpha = 0;

        // Fade in text
        cg.DOFade(1f, fadeDuration);

        // Scroll up
        scrollTween = creditRect
            .DOAnchorPos(endPos, scrollDuration)
            .SetEase(Ease.Linear)
            .OnComplete(OnCreditFinished);
    }

    void Update()
    {
        if (!allowSkip) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SkipCredits();
        }
    }


    void SkipCredits()
    {
        scrollTween.Kill();
        OnCreditFinished();
    }

    void OnCreditFinished()
    {
        Debug.Log("CREDITS FINISHED");
        // TODO:
        SceneManager.LoadScene(0);
        // Application.Quit();
    }

    CanvasGroup GetOrAddCanvasGroup(Component obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = obj.gameObject.AddComponent<CanvasGroup>();
        return cg;
    }
}
