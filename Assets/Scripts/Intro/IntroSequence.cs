using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class IntroSequence : MonoBehaviour
{
    [Header("UI References")]
    public Image studioLogo;
    public Image gameLogo;
    public TextMeshProUGUI transitionText;
    public GameObject skipPrompt; // Optional UI element to show "Press any key to skip"
    public Image screenFadeOverlay; // Black overlay for screen fades
    public Image vignetteOverlay; // Vignette effect overlay
    public RawImage scanlineOverlay; // Optional scanline/CRT effect
    public Slider progressBar; // Optional loading progress bar

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip beepAudioClip;
    public AudioClip keyStrokeAudioClip;
    public AudioClip ambientDroneClip; // Background atmospheric sound
    public AudioClip glitchSoundClip; // Sound during glitch effect
    [Range(0f, 1f)] public float beepVolume = 1f;
    [Range(0f, 1f)] public float keyStrokeVolume = 0.6f;
    [Range(0f, 1f)] public float ambientVolume = 0.3f;
    [Range(0f, 1f)] public float glitchVolume = 0.5f;

    [Header("Scene Settings")]
    public string nextScene = "MainMenu";
    public bool allowSkip = true;
    public KeyCode skipKey = KeyCode.Space;
    public bool skipOnAnyKey = true;
    public bool useAsyncSceneLoading = true; // Show loading progress

    [Header("Timing Configuration")]
    [Range(0.1f, 3f)] public float studioLogoFadeInDuration = 0.8f;
    [Range(0.5f, 5f)] public float studioLogoDisplayDuration = 1.4f;
    [Range(0.1f, 3f)] public float studioLogoFadeOutDuration = 0.8f;
    [Range(0.01f, 0.2f)] public float typewriterSpeed = 0.05f;
    [Range(0.5f, 5f)] public float transitionTextDisplayDuration = 1.6f;
    [Range(0.5f, 5f)] public float gameLogoDisplayDuration = 1.5f;
    [Range(0.1f, 3f)] public float gameLogoFadeInDuration = 1f;

    [Header("Effect Settings")]
    public bool enableGlitchEffect = true;
    [Range(1, 10)] public int glitchFlashCount = 5;
    [Range(0.5f, 5f)] public float glitchIntensity = 2f;
    public bool enableTypewriterSound = true;
    public bool enableRandomMessage = true;
    public int specificMessageIndex = 0; // Used when enableRandomMessage is false

    [Header("Cinematic Effects")]
    public bool enableScreenFade = true;
    [Range(0.5f, 3f)] public float screenFadeDuration = 1f;
    public bool enableVignetteEffect = true;
    [Range(0f, 1f)] public float vignetteIntensity = 0.5f;
    public bool enableScanlineEffect = false;
    [Range(0.5f, 5f)] public float scanlineSpeed = 2f;
    public bool enableChromaticAberration = true;
    [Range(0f, 5f)] public float chromaticAberrationAmount = 2f;
    public bool enableCameraShake = true;
    [Range(0.1f, 2f)] public float shakeIntensity = 0.5f;
    public bool enableAmbientSound = true;
    public bool enableAudioFade = true;
    public bool enableTextShadowPulse = true;
    [Range(0.5f, 3f)] public float textPulseSpeed = 1.5f;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent OnSequenceStart;
    public UnityEngine.Events.UnityEvent OnSequenceComplete;
    public UnityEngine.Events.UnityEvent OnSequenceSkipped;

    private string[] messages = new string[]
    {
        "Protocol Shift…",
        "Subject Transfer…",
        "Environment Sync Complete.",
        "Handoff Successful.",
        "Experiment Continues…"
    };

    private bool isSkipped = false;
    private Coroutine sequenceCoroutine;
    private Camera mainCamera;
    private Vector3 originalCameraPosition;
    private AudioSource ambientAudioSource;

    void Start()
    {
        // CRITICAL: Hide all UI elements IMMEDIATELY to prevent visible glitching
        // This must happen before any other initialization
        if (studioLogo != null)
            studioLogo.canvasRenderer.SetAlpha(0);
        if (gameLogo != null)
            gameLogo.canvasRenderer.SetAlpha(0);
        if (transitionText != null)
        {
            transitionText.alpha = 0;
            transitionText.text = "";
        }

        audioSource = GetComponent<AudioSource>();

        // Setup camera reference for shake effect
        mainCamera = Camera.main;
        if (mainCamera != null)
            originalCameraPosition = mainCamera.transform.localPosition;

        // Setup ambient audio source
        if (enableAmbientSound && ambientDroneClip != null)
        {
            ambientAudioSource = gameObject.AddComponent<AudioSource>();
            ambientAudioSource.clip = ambientDroneClip;
            ambientAudioSource.loop = true;
            ambientAudioSource.volume = enableAudioFade ? 0f : ambientVolume;
            ambientAudioSource.Play();
        }

        // Hide skip prompt initially
        if (skipPrompt != null)
            skipPrompt.SetActive(allowSkip);

        // Setup progress bar
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
            progressBar.value = 0f;
        }

        // Initialize vignette
        if (vignetteOverlay != null)
        {
            vignetteOverlay.canvasRenderer.SetAlpha(enableVignetteEffect ? vignetteIntensity : 0f);
        }

        // Initialize screen fade overlay to black if screen fade is enabled
        if (enableScreenFade && screenFadeOverlay != null)
        {
            screenFadeOverlay.canvasRenderer.SetAlpha(1f);
        }

        sequenceCoroutine = StartCoroutine(RunSequence());
    }

    void Update()
    {
        // Handle skip input
        if (allowSkip && !isSkipped)
        {
            bool skipPressed = Input.GetKeyDown(skipKey);

            if (skipOnAnyKey && Input.anyKeyDown && !skipPressed)
                skipPressed = true;

            if (skipPressed)
            {
                SkipSequence();
            }
        }

        // Animate scanlines
        if (enableScanlineEffect && scanlineOverlay != null)
        {
            var uvRect = scanlineOverlay.uvRect;
            uvRect.y += Time.deltaTime * scanlineSpeed * 0.1f;
            scanlineOverlay.uvRect = uvRect;
        }
    }

    void SkipSequence()
    {
        if (isSkipped) return;

        isSkipped = true;

        // Stop current sequence
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        // Stop all coroutines to prevent any lingering effects
        StopAllCoroutines();

        // Reset camera position
        if (mainCamera != null && enableCameraShake)
            mainCamera.transform.localPosition = originalCameraPosition;

        // Trigger skip event
        OnSequenceSkipped?.Invoke();

        Debug.Log("Intro sequence skipped by user");

        // Load next scene immediately
        LoadNextScene();
    }

    IEnumerator RunSequence()
    {
        // Trigger start event
        OnSequenceStart?.Invoke();

        // Fade in ambient audio
        if (enableAudioFade && ambientAudioSource != null)
        {
            StartCoroutine(FadeAudio(ambientAudioSource, ambientVolume, 2f));
        }

        // Initial screen fade from black
        if (enableScreenFade && screenFadeOverlay != null)
        {
            screenFadeOverlay.CrossFadeAlpha(0f, screenFadeDuration, false);
            yield return new WaitForSeconds(screenFadeDuration);
        }

        // Run sequence parts
        yield return StudioLogo();
        if (isSkipped) yield break;

        yield return TransitionText();
        if (isSkipped) yield break;

        yield return GameLogo();
        if (isSkipped) yield break;

        // Show loading progress if enabled
        if (useAsyncSceneLoading && progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            yield return StartCoroutine(LoadSceneAsync());
        }
        else
        {
            // Auto load after logo displayed
            yield return new WaitForSeconds(gameLogoDisplayDuration);

            if (!isSkipped)
            {
                // Fade out audio
                if (enableAudioFade && ambientAudioSource != null)
                {
                    StartCoroutine(FadeAudio(ambientAudioSource, 0f, 1f));
                }

                // Fade to black before transition
                if (enableScreenFade && screenFadeOverlay != null)
                {
                    screenFadeOverlay.CrossFadeAlpha(1f, screenFadeDuration, false);
                    yield return new WaitForSeconds(screenFadeDuration);
                }

                OnSequenceComplete?.Invoke();
                LoadNextScene();
            }
        }
    }

    IEnumerator StudioLogo()
    {
        if (studioLogo == null)
        {
            Debug.LogWarning("Studio Logo reference is missing!");
            yield break;
        }

        // Camera shake on logo appearance
        if (enableCameraShake && mainCamera != null)
        {
            StartCoroutine(CameraShake(0.3f, shakeIntensity * 0.5f));
        }

        studioLogo.CrossFadeAlpha(1f, studioLogoFadeInDuration, false);

        // Add chromatic aberration effect during fade in
        if (enableChromaticAberration)
        {
            StartCoroutine(ChromaticAberrationPulse(studioLogo.rectTransform, studioLogoFadeInDuration));
        }

        yield return new WaitForSeconds(studioLogoDisplayDuration);
        studioLogo.CrossFadeAlpha(0f, studioLogoFadeOutDuration, false);
        yield return new WaitForSeconds(studioLogoFadeOutDuration);
    }

    IEnumerator TransitionText()
    {
        if (transitionText == null)
        {
            Debug.LogWarning("Transition Text reference is missing!");
            yield break;
        }

        // Select message
        string msg = enableRandomMessage
            ? messages[UnityEngine.Random.Range(0, messages.Length)]
            : messages[Mathf.Clamp(specificMessageIndex, 0, messages.Length - 1)];

        transitionText.text = "";
        transitionText.alpha = 1f;

        // Small glitch flash before reveal
        if (enableGlitchEffect)
            yield return StartCoroutine(GlitchFlash());

        // Start text pulse effect
        if (enableTextShadowPulse)
        {
            StartCoroutine(TextPulseEffect());
        }

        // Typewriter reveal
        yield return StartCoroutine(Typewriter(msg, typewriterSpeed));

        // Stay longer on screen
        yield return new WaitForSeconds(transitionTextDisplayDuration);

        // fade out
        transitionText.CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator GameLogo()
    {
        if (gameLogo == null)
        {
            Debug.LogWarning("Game Logo reference is missing!");
            yield break;
        }

        // Dramatic camera shake for game logo
        if (enableCameraShake && mainCamera != null)
        {
            StartCoroutine(CameraShake(0.5f, shakeIntensity));
        }

        gameLogo.CrossFadeAlpha(1f, gameLogoFadeInDuration, false);

        // Play beep sound for logo appearance
        if (audioSource != null && beepAudioClip != null)
        {
            audioSource.PlayOneShot(beepAudioClip, beepVolume);
        }

        // Add chromatic aberration effect
        if (enableChromaticAberration)
        {
            StartCoroutine(ChromaticAberrationPulse(gameLogo.rectTransform, gameLogoFadeInDuration * 0.5f));
        }

        yield return null;
    }

    IEnumerator Typewriter(string fullText, float delay)
    {
        if (transitionText == null) yield break;

        transitionText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            if (isSkipped) yield break;

            transitionText.text += fullText[i];

            // Play keystroke sound for each character
            if (enableTypewriterSound && audioSource != null && keyStrokeAudioClip != null)
            {
                audioSource.PlayOneShot(keyStrokeAudioClip, keyStrokeVolume);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator GlitchFlash()
    {
        if (transitionText == null) yield break;

        // Store original position
        Vector3 originalPosition = transitionText.rectTransform.localPosition;

        // Play glitch sound
        if (audioSource != null && glitchSoundClip != null)
        {
            audioSource.PlayOneShot(glitchSoundClip, glitchVolume);
        }

        // quick glitch before reveal
        transitionText.alpha = 1;

        for (int i = 0; i < glitchFlashCount; i++)
        {
            if (isSkipped) break;

            transitionText.rectTransform.localPosition = originalPosition +
                new Vector3(UnityEngine.Random.Range(-glitchIntensity, glitchIntensity),
                           UnityEngine.Random.Range(-glitchIntensity, glitchIntensity), 0);

            transitionText.alpha = UnityEngine.Random.value > 0.5f ? 1f : 0.3f;

            // Random color shift during glitch
            if (enableChromaticAberration)
            {
                Color glitchColor = new Color(
                    UnityEngine.Random.value,
                    UnityEngine.Random.value,
                    UnityEngine.Random.value,
                    1f
                );
                transitionText.color = glitchColor;
            }

            yield return new WaitForSeconds(0.05f);
        }

        // reset to original position and color
        transitionText.alpha = 1;
        transitionText.rectTransform.localPosition = originalPosition;
        transitionText.color = Color.white;
    }

    // NEW CINEMATIC EFFECTS

    IEnumerator CameraShake(float duration, float intensity)
    {
        if (mainCamera == null) yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (isSkipped) break;

            float x = UnityEngine.Random.Range(-1f, 1f) * intensity;
            float y = UnityEngine.Random.Range(-1f, 1f) * intensity;

            mainCamera.transform.localPosition = originalCameraPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalCameraPosition;
    }

    IEnumerator ChromaticAberrationPulse(RectTransform target, float duration)
    {
        float elapsed = 0f;
        Vector3 originalScale = target.localScale;

        while (elapsed < duration)
        {
            if (isSkipped) break;

            float pulse = Mathf.Sin(elapsed * 10f) * chromaticAberrationAmount * 0.01f;
            target.localScale = originalScale + Vector3.one * pulse;

            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = originalScale;
    }

    IEnumerator TextPulseEffect()
    {
        if (transitionText == null) yield break;

        float elapsed = 0f;
        Color originalColor = transitionText.color;

        while (transitionText.alpha > 0.1f && !isSkipped)
        {
            float pulse = Mathf.PingPong(elapsed * textPulseSpeed, 1f);
            transitionText.color = Color.Lerp(originalColor, originalColor * 1.2f, pulse * 0.3f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transitionText.color = originalColor;
    }

    IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        if (source == null) yield break;

        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
    }

    IEnumerator LoadSceneAsync()
    {
        // Fade out audio
        if (enableAudioFade && ambientAudioSource != null)
        {
            StartCoroutine(FadeAudio(ambientAudioSource, 0f, 1f));
        }

        // Fade to black before transition
        if (enableScreenFade && screenFadeOverlay != null)
        {
            screenFadeOverlay.CrossFadeAlpha(1f, screenFadeDuration, false);
            yield return new WaitForSeconds(screenFadeDuration * 0.5f);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        // Update progress bar
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            // Scene is ready
            if (asyncLoad.progress >= 0.9f)
            {
                // Wait a moment for effect
                yield return new WaitForSeconds(0.5f);

                OnSequenceComplete?.Invoke();
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }

    void OnDestroy()
    {
        // Cleanup camera position
        if (mainCamera != null && enableCameraShake)
        {
            mainCamera.transform.localPosition = originalCameraPosition;
        }

        // Cleanup ambient audio
        if (ambientAudioSource != null)
        {
            Destroy(ambientAudioSource);
        }
    }

    // Public methods for external control
    public void SetNextScene(string sceneName)
    {
        nextScene = sceneName;
    }

    public void AddCustomMessage(string message)
    {
        var messageList = new System.Collections.Generic.List<string>(messages);
        messageList.Add(message);
        messages = messageList.ToArray();
    }

    public void ForceSkip()
    {
        SkipSequence();
    }

    public void SetVignetteIntensity(float intensity)
    {
        vignetteIntensity = Mathf.Clamp01(intensity);
        if (vignetteOverlay != null)
        {
            vignetteOverlay.canvasRenderer.SetAlpha(vignetteIntensity);
        }
    }
}
