using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class EndOfDayUI : MonoBehaviour
{
    public static EndOfDayUI Instance { get; private set; }

    [Header("Panel Next")]
    public GameObject PanelDay;
    public RectTransform PanelNewsNormal;
    public RectTransform PanelNewsWanted;

    [Header("Text")]
    public TMP_Text TextNews;
    public TMP_Text TextDay;

    [Header("Button")]
    public Button nextDay;

    [Header("Animation")]
    public float duration = 0.4f;
    public Ease openEase = Ease.OutCubic;
    public Ease closeEase = Ease.InCubic;

    Vector2 onScreenPos;
    Vector2 offScreenPos;

    public int Results;

    bool isAnimating;
    private void Awake()
    {
        if (Instance == null)
        {
            nextDay.gameObject.SetActive(false);
            Instance = this;
            DontDestroyOnLoad(gameObject);
           
            offScreenPos = onScreenPos + new Vector2(0, -Screen.height);

        
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDay()
    {
        PanelDay.gameObject.SetActive(true);
        TextDay.text = $"Day {NPCQueueManager.Instance.CurrentDay + 1}";

        StartCoroutine(ShowResultNew());
    }
    
    IEnumerator ShowResultNew()
    {
        yield return new WaitForSeconds(2f);
        ShowResult(Results);
    }

    public void NextDay()
    {
        NPCQueueManager.Instance.StartDay();
        PanelDay.gameObject.SetActive(false);
        PanelNewsWanted.gameObject.SetActive(false);
        PanelNewsNormal.gameObject.SetActive(false);
        nextDay.gameObject.SetActive(false);
    }

    public void ShowResult(int Result)
    {
        if (Result > 0)
        {
            PanelNewsWanted.gameObject.SetActive(true);
            PanelNewsNormal.gameObject.SetActive(false);
            TextNews.text = $"Berita Terkini {Result} Kriminal Pemalsuan Emosi Tertangkap";
            nextDay.gameObject.SetActive(true);
        }
        else
        {
            nextDay.gameObject.SetActive(true);
            PanelNewsWanted.gameObject.SetActive(false);
            PanelNewsNormal.gameObject.SetActive(true);
        }
    }
}
