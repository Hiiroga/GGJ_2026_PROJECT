using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }
    public RectTransform PanelBg;
    public RectTransform panelNotes;
    public RectTransform panelNotes2;

    [Header("Animation")]
    public float animDuration = 0.4f;
    public Ease showEase = Ease.OutBack;
    public Ease switchEase = Ease.OutCubic;

    Vector2 centerPos;
    Vector2 offBottomPos;
    Vector2 offLeftPos;
    Vector2 offRightPos;

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

    void Start()
    {
        centerPos = panelNotes.anchoredPosition;

        offBottomPos = centerPos + new Vector2(0, -Screen.height);
        offLeftPos = centerPos + new Vector2(-Screen.width, 0);
        offRightPos = centerPos + new Vector2(Screen.width, 0);

        panelNotes.anchoredPosition = offBottomPos;
        panelNotes2.anchoredPosition = offBottomPos;

        PanelBg.gameObject.SetActive(false);
        panelNotes.gameObject.SetActive(false);
        panelNotes2.gameObject.SetActive(false);
    }

    public void ShowNotes()
    {
        PanelBg.gameObject.SetActive(true);
        panelNotes.gameObject.SetActive(true);
        panelNotes.anchoredPosition = offBottomPos;

        panelNotes
            .DOAnchorPos(centerPos, animDuration)
            .SetEase(showEase);
    }

    public void HideNotes()
    {
        PanelBg.gameObject.SetActive(false);
        RectTransform current = panelNotes.gameObject.activeSelf
            ? panelNotes
            : panelNotes2;

        current
            .DOAnchorPos(offBottomPos, animDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                current.gameObject.SetActive(false);
            });
    }

    public void NextNotes()
    {
        SwitchHorizontal(panelNotes, panelNotes2, true);
    }

    public void PreviousNotes()
    {
        SwitchHorizontal(panelNotes2, panelNotes, false);
    }

    void SwitchHorizontal(RectTransform from, RectTransform to, bool toLeft)
    {
        to.gameObject.SetActive(true);

        Vector2 fromTarget = toLeft ? offLeftPos : offRightPos;
        Vector2 toStart = toLeft ? offRightPos : offLeftPos;

        to.anchoredPosition = toStart;

        from
            .DOAnchorPos(fromTarget, animDuration)
            .SetEase(switchEase)
            .OnComplete(() =>
            {
                from.gameObject.SetActive(false);
            });

        to
            .DOAnchorPos(centerPos, animDuration)
            .SetEase(switchEase);
    }
}
