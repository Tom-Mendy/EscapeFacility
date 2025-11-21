using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DetectionUI : MonoBehaviour
{
    [Header("Detection Bar")]
    [SerializeField] private GameObject detectionBarPanel;
    [SerializeField] private Image detectionFillImage;
    [SerializeField] private Color detectionColorStart = Color.yellow;
    [SerializeField] private Color detectionColorEnd = Color.red;
    [SerializeField] private float detectionSpeed = 0.5f; // Speed of detection fill
    [SerializeField] private float detectionDecaySpeed = 1f; // Speed of detection decay when not detected

    [Header("Caught Screen")]
    [SerializeField] private GameObject caughtPanel;
    [SerializeField] private Image caughtScreenImage;
    [SerializeField] private TMP_Text caughtText;
    [SerializeField] private float caughtFlashDuration = 2f;

    [Header("Guard References")]
    [SerializeField] private Transform[] guards;

    private float currentDetectionLevel = 0f;
    private bool isDetected = false;
    private bool isCaught = false;
    private float caughtTimer = 0f;

    void Start()
    {
        // Validate references
        if (detectionBarPanel == null)
        {
            Debug.LogError("DetectionUI: Must reference the Detection Bar Panel");
        }
        if (detectionFillImage == null)
        {
            Debug.LogError("DetectionUI: Must reference the Detection Fill Image");
        }
        if (caughtPanel == null)
        {
            Debug.LogError("DetectionUI: Must reference the Caught Panel");
        }

        // Initialize UI state
        if (detectionBarPanel != null)
        {
            detectionBarPanel.SetActive(false);
        }
        if (caughtPanel != null)
        {
            caughtPanel.SetActive(false);
        }

        // Find guards if not assigned
        if (guards == null || guards.Length == 0)
        {
            GameObject[] guardObjects = GameObject.FindGameObjectsWithTag("Guard");
            guards = new Transform[guardObjects.Length];
            for (int i = 0; i < guardObjects.Length; i++)
            {
                guards[i] = guardObjects[i].transform;
            }
            if (guards.Length == 0)
            {
                Debug.LogWarning("DetectionUI: No guards found in the scene. Tag guards with 'Guard' or assign manually.");
            }
        }

        // Setup caught text
        if (caughtText != null)
        {
            caughtText.text = "CAUGHT!";
        }
    }

    void Update()
    {
        if (isCaught)
        {
            HandleCaughtState();
            return;
        }

        UpdateDetectionLevel();
        UpdateDetectionBar();
    }

    void UpdateDetectionLevel()
    {
        bool anyGuardSeesPlayer = false;

        // Check if any guard can see the player
        if (guards != null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                foreach (Transform guard in guards)
                {
                    if (guard != null)
                    {
                        // Check distance and line of sight
                        float distanceToPlayer = Vector3.Distance(guard.position, playerObj.transform.position);

                        // Simple detection: check if guard is close and can see player
                        if (distanceToPlayer < 15f) // Detection range
                        {
                            Vector3 directionToPlayer = (playerObj.transform.position - guard.position).normalized;
                            float angle = Vector3.Angle(guard.forward, directionToPlayer);

                            if (angle < 45f) // Detection cone (90 degrees total)
                            {
                                // Raycast to check line of sight
                                RaycastHit hit;
                                if (Physics.Raycast(guard.position + Vector3.up, directionToPlayer, out hit, distanceToPlayer))
                                {
                                    if (hit.collider.CompareTag("Player"))
                                    {
                                        anyGuardSeesPlayer = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        isDetected = anyGuardSeesPlayer;

        // Update detection level
        if (isDetected)
        {
            currentDetectionLevel += detectionSpeed * Time.deltaTime;
            currentDetectionLevel = Mathf.Clamp01(currentDetectionLevel);

            // Show detection bar when being detected
            if (detectionBarPanel != null && !detectionBarPanel.activeSelf)
            {
                detectionBarPanel.SetActive(true);
            }

            // Check if caught
            if (currentDetectionLevel >= 1f && !isCaught)
            {
                TriggerCaught();
            }
        }
        else
        {
            // Decay detection level when not detected
            if (currentDetectionLevel > 0f)
            {
                currentDetectionLevel -= detectionDecaySpeed * Time.deltaTime;
                currentDetectionLevel = Mathf.Max(0f, currentDetectionLevel);
            }

            // Hide detection bar when detection is zero
            if (currentDetectionLevel <= 0f && detectionBarPanel != null && detectionBarPanel.activeSelf)
            {
                detectionBarPanel.SetActive(false);
            }
        }
    }

    void UpdateDetectionBar()
    {
        if (detectionFillImage != null)
        {
            detectionFillImage.fillAmount = currentDetectionLevel;
            detectionFillImage.color = Color.Lerp(detectionColorStart, detectionColorEnd, currentDetectionLevel);
        }
    }

    void TriggerCaught()
    {
        isCaught = true;
        caughtTimer = 0f;

        // Show caught screen
        if (caughtPanel != null)
        {
            caughtPanel.SetActive(true);
        }

        // Hide detection bar
        if (detectionBarPanel != null)
        {
            detectionBarPanel.SetActive(false);
        }

        // Pause the game
        Time.timeScale = 0f;

        Debug.Log("Player caught!");
    }

    void HandleCaughtState()
    {
        caughtTimer += Time.unscaledDeltaTime;

        // Flash effect on caught screen
        if (caughtScreenImage != null)
        {
            float alpha = Mathf.PingPong(caughtTimer * 2f, 0.5f) + 0.3f;
            Color color = caughtScreenImage.color;
            color.a = alpha;
            caughtScreenImage.color = color;
        }

        // Optional: Auto-restart or return to menu after duration
        if (caughtTimer >= caughtFlashDuration)
        {
            // Could trigger game over screen or restart here
            // For now, just keep showing the caught screen
        }
    }

    // Public method to reset detection (useful for respawn or restart)
    public void ResetDetection()
    {
        currentDetectionLevel = 0f;
        isDetected = false;
        isCaught = false;
        caughtTimer = 0f;

        if (detectionBarPanel != null)
        {
            detectionBarPanel.SetActive(false);
        }
        if (caughtPanel != null)
        {
            caughtPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    // Public method to manually set caught state (called by guards or other systems)
    public void SetCaught()
    {
        if (!isCaught)
        {
            currentDetectionLevel = 1f;
            TriggerCaught();
        }
    }

    // Property to check if player is caught
    public bool IsCaught => isCaught;

    // Property to get current detection level (0-1)
    public float DetectionLevel => currentDetectionLevel;
}
