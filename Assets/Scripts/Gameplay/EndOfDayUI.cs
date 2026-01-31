using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class EndOfDayUI : MonoBehaviour
{
    public static EndOfDayUI Instance { get; private set; }

    [Header("Panels")]
    public RectTransform panelDay;
    public RectTransform panelNewsNormal;
    public RectTransform panelNewsWanted;

    [Header("Text")]
    public TMP_Text textNews;
    public TMP_Text textDay;

    [Header("Button")]
    public Button nextDay;

    [Header("Animation")]
    public float duration = 0.4f;
    public Ease openEase = Ease.OutCubic;
    public Ease closeEase = Ease.InCubic;

    Vector2 centerPos;
    Vector2 offBottomPos;

    public int Results;
    bool isAnimating;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // INIT POSITIONS
        centerPos = panelDay.anchoredPosition;
        offBottomPos = centerPos + new Vector2(0, -Screen.height);

        panelDay.anchoredPosition = offBottomPos;
        panelDay.gameObject.SetActive(false);

        panelNewsNormal.gameObject.SetActive(false);
        panelNewsWanted.gameObject.SetActive(false);
        nextDay.gameObject.SetActive(false);
    }

    // ================= SHOW DAY =================

    public void ShowDay()
    {
        if (isAnimating) return;
        isAnimating = true;

        textDay.text = $"Day {NPCQueueManager.Instance.CurrentDay + 1}";
        textDay.alpha = 0;

        panelDay.gameObject.SetActive(true);
        panelDay.anchoredPosition = offBottomPos;

        panelDay
            .DOAnchorPos(centerPos, duration)
            .SetEase(openEase)
            .OnComplete(() =>
            {
                AnimateDayText();
            });
    }

    void AnimateDayText()
    {
        textDay.transform.localPosition -= new Vector3(0, 20, 0);

        DOTween.Sequence()
            .Append(textDay.DOFade(1f, 0.3f))
            .Join(textDay.transform.DOLocalMoveY(0, 0.3f))
            .AppendInterval(0.6f)
            .OnComplete(() =>
            {
                StartCoroutine(ShowResultNew());
            });
    }

    IEnumerator ShowResultNew()
    {
        yield return new WaitForSeconds(0.3f);
        ShowResult(Results);
    }

    // ================= RESULT =================

    public void ShowResult(int result)
    {
        RectTransform panelToShow =
            result > 0 ? panelNewsWanted : panelNewsNormal;

        panelNewsNormal.gameObject.SetActive(false);
        panelNewsWanted.gameObject.SetActive(false);

        panelToShow.gameObject.SetActive(true);
        panelToShow.localScale = Vector3.one * 0.95f;

        CanvasGroup cg = GetOrAddCanvasGroup(panelToShow);
        cg.alpha = 0;

        if (result > 0)
            textNews.text = $"Berita Terkini {result} Kriminal Pemalsuan Emosi Tertangkap";

        DOTween.Sequence()
            .Append(cg.DOFade(1f, duration * 0.8f))
            .Join(panelToShow.DOScale(1f, duration))
            .OnComplete(() =>
            {
                ShowNextButton();
            });
    }

    void ShowNextButton()
    {
        nextDay.gameObject.SetActive(true);

        CanvasGroup cg = GetOrAddCanvasGroup(nextDay.transform);
        cg.alpha = 0;

        cg.DOFade(1f, 0.25f)
          .OnComplete(() => isAnimating = false);
    }

    // ================= NEXT DAY =================

    public void NextDay()
    {
        if (isAnimating) return;
        isAnimating = true;

        panelDay
            .DOAnchorPos(offBottomPos, duration)
            .SetEase(closeEase)
            .OnComplete(() =>
            {
                panelDay.gameObject.SetActive(false);
                panelNewsNormal.gameObject.SetActive(false);
                panelNewsWanted.gameObject.SetActive(false);
                nextDay.gameObject.SetActive(false);

                isAnimating = false;
                NPCQueueManager.Instance.StartDay();
            });
    }

    // ================= HELPER =================

    CanvasGroup GetOrAddCanvasGroup(Component obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = obj.gameObject.AddComponent<CanvasGroup>();
        return cg;
    }
}
