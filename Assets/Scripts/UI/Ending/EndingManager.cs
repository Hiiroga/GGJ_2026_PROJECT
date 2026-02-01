using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class EndingManager : MonoBehaviour
{
    [Header("Panels")]
    public RectTransform endingWin;
    public RectTransform endingLose;

    public GameObject endingLosePrefab;
    public GameObject endingWinPrefab;

    [Header("Text Renderer")]
    public TMP_Text textWin;
    public TMP_Text textLose;

    [Header("Story Text (Editable in Inspector)")]
    [TextArea(5, 10)]
    public string winStory;

    [TextArea(5, 10)]
    public string loseStory;

    [Header("Typewriter")]
    public float typeSpeed = 0.04f;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip typeSfx;
    public int soundEveryNChar = 2;

    [Header("Fade")]
    public float fadeDuration = 1.2f;
    public int winThreshold = 4;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("Score", 0);
        Debug.Log(finalScore);
        bool isLose = finalScore > winThreshold;

        RectTransform panel = isLose ? endingLose : endingWin;
        TMP_Text text = isLose ? textLose : textWin;
        string story = isLose ? loseStory : winStory;

        if (isLose)
        {
            endingLosePrefab.SetActive(true);
            endingWinPrefab.SetActive(false);
        }
        else 
        {
            endingLosePrefab.SetActive(false);
            endingWinPrefab.SetActive(true);
        }

        // pastikan panel aktif tapi invisible
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = panel.gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        panel.gameObject.SetActive(true);



        text.text = ""; // kosongkan renderer

        StartCoroutine(PlayEnding(text, panel, story));
    }

    IEnumerator PlayEnding(TMP_Text text, RectTransform panel, string story)
    {
        // === TYPEWRITER ===
        yield return StartCoroutine(Typewriter(text, story));

        yield return new WaitForSeconds(0.4f);

        // === FADE IN PANEL ===
        FadeInPanel(panel);
    }

    IEnumerator Typewriter(TMP_Text text, string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            text.text = fullText.Substring(0, i);

            if (i % soundEveryNChar == 0 && audioSource && typeSfx)
            {
                audioSource.PlayOneShot(typeSfx);
            }

            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void FadeInPanel(RectTransform panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        cg.DOFade(1f, fadeDuration)
          .SetEase(Ease.OutQuad);

        StartCoroutine(BackToMainMenu());
    }

    IEnumerator BackToMainMenu()
    {
        Destroy(EmotionManager.Instance.gameObject);
        Destroy(NPCQueueManager.Instance.gameObject);
        Destroy(NotesManager.Instance.gameObject);
        Destroy(DialogueManager.Instance.gameObject);
        Destroy(EndOfDayUI.Instance.gameObject);
        Destroy(CraftingManager.Instance.gameObject);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
