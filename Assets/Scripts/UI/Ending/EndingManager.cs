using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    public RectTransform endingWin;
    public RectTransform endingLose;

    public TMP_Text textWin;
    public TMP_Text textLose;

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
        bool isLose = finalScore > winThreshold;

        RectTransform panel = isLose ? endingLose : endingWin;
        TMP_Text text = isLose ? textLose : textWin;

        // hide panel dulu
        panel.gameObject.SetActive(false);

        StartCoroutine(PlayEnding(text, panel));
    }

    IEnumerator PlayEnding(TMP_Text text, RectTransform panel)
    {
        // === TYPEWRITER DULU ===
        string fullText = text.text;
        text.text = "";

        yield return StartCoroutine(Typewriter(text, fullText));

        // jeda dikit biar napas
        yield return new WaitForSeconds(0.4f);

        // === BARU PANEL FADE-IN ===
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
        panel.gameObject.SetActive(true);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = panel.gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0f;

        cg.DOFade(1f, fadeDuration)
          .SetEase(Ease.OutQuad);
    }
}
