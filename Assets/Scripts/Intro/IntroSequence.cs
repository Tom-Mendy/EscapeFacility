using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class IntroSequence : MonoBehaviour
{
    [Header("UI References")]
    public Image studioLogo;
    public Image gameLogo;
    public TextMeshProUGUI transitionText;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip beepAudioClip;
    public AudioClip keyStrokeAudioClip;

    [Header("Scene Settings")]
    public string nextScene = "MainMenu";

    private string[] messages = new string[]
    {
        "Protocol Shift…",
        "Subject Transfer…",
        "Environment Sync Complete.",
        "Handoff Successful.",
        "Experiment Continues…"
    };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        // Initialize
        studioLogo.canvasRenderer.SetAlpha(0);
        gameLogo.canvasRenderer.SetAlpha(0);
        transitionText.alpha = 0;

        yield return StudioLogo();
        yield return TransitionText();
        yield return GameLogo();

        // Auto load after logo displayed
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(nextScene);
    }

    IEnumerator StudioLogo()
    {
        studioLogo.CrossFadeAlpha(1f, 0.8f, false);
        yield return new WaitForSeconds(1.4f);
        studioLogo.CrossFadeAlpha(0f, 0.8f, false);
        yield return new WaitForSeconds(0.8f);
    }

    IEnumerator TransitionText()
    {
        string msg = messages[Random.Range(0, messages.Length)];
        transitionText.text = "";
        transitionText.alpha = 1f;

        // Small glitch flash before reveal
        yield return StartCoroutine(GlitchFlash());

        // Typewriter reveal
        yield return StartCoroutine(Typewriter(msg, 0.05f));

        // Stay longer on screen
        yield return new WaitForSeconds(1.6f);

        // fade out
        transitionText.CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator GameLogo()
    {
        gameLogo.CrossFadeAlpha(1f, 1f, false);
        // Play key Stroke for each character
        if (audioSource != null && beepAudioClip != null)
        {
            audioSource.PlayOneShot(beepAudioClip, 1f);
        }
        yield return null;
    }

    IEnumerator Typewriter(string fullText, float delay)
    {
        transitionText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            transitionText.text += fullText[i];

            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator GlitchFlash()
    {
        // quick glitch before reveal
        transitionText.alpha = 1;

        for (int i = 0; i < 5; i++)
        {
            transitionText.rectTransform.localPosition =
                new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);

            transitionText.alpha = Random.value > 0.5f ? 1f : 0.3f;

            yield return new WaitForSeconds(0.05f);
        }

        // reset
        transitionText.alpha = 1;
        transitionText.rectTransform.localPosition = Vector3.zero;
    }
}
