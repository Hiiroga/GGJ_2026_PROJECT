using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PrologPanel
{
    public RectTransform panel;
    public TMP_Text text;
    [TextArea(3, 6)]
    public string[] sentences;
}

public class PrologManager : MonoBehaviour
{
    [Header("Panels")]
    public PrologPanel[] prologPanels;

    [Header("Typewriter")]
    public float typeSpeed = 0.04f;
    public float sentenceDelay = 0.6f;

    [Header("Fade")]
    public float panelFadeDuration = 0.8f;
    public float textFadeDuration = 0.4f;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip typeSfx;
    public int soundEveryNChar = 2;

    void Start()
    {
        StartCoroutine(PlayProlog());
    }

    IEnumerator PlayProlog()
    {
        // hide all panels first
        foreach (var p in prologPanels)
        {
            p.panel.gameObject.SetActive(false);
        }

        // play panels one by one
        foreach (var p in prologPanels)
        {
            yield return StartCoroutine(PlayPanel(p));
        }

        // TODO: lanjut ke scene gameplay
        MusicManager.Instance.Play("ingame");
        SceneManager.LoadScene(2);
    }

    IEnumerator PlayPanel(PrologPanel p)
    {
        // === INIT PANEL ===
        p.panel.gameObject.SetActive(true);

        CanvasGroup panelCG = GetOrAddCanvasGroup(p.panel);
        panelCG.alpha = 0;

        p.text.text = "";
        CanvasGroup textCG = GetOrAddCanvasGroup(p.text);
        textCG.alpha = 1;

        // === FADE IN PANEL ===
        panelCG.DOFade(1f, panelFadeDuration);
        yield return new WaitForSeconds(panelFadeDuration + 0.2f);

        // === SENTENCES ===
        foreach (string sentence in p.sentences)
        {
            // typewriter
            yield return StartCoroutine(Typewriter(p.text, sentence));

            yield return new WaitForSeconds(sentenceDelay);

            // fade out text
            textCG.DOFade(0f, textFadeDuration);
            yield return new WaitForSeconds(textFadeDuration);

            // reset for next sentence
            p.text.text = "";
            textCG.alpha = 1f;
        }

        // === FADE OUT PANEL ===
        panelCG.DOFade(0f, panelFadeDuration);
        yield return new WaitForSeconds(panelFadeDuration);

        p.panel.gameObject.SetActive(false);
    }

    IEnumerator Typewriter(TMP_Text text, string fullText)
    {
        text.text = "";

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

    CanvasGroup GetOrAddCanvasGroup(Component obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = obj.gameObject.AddComponent<CanvasGroup>();
        return cg;
    }
}
