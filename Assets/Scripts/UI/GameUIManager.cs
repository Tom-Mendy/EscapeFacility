using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestionnaire principal de l'UI du jeu
/// Coordonne la barre de détection, la mini-map, les caméras, et le menu pause
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private DetectionUI detectionUI;
    [SerializeField] private MiniMapCamera miniMapCamera;
    [SerializeField] private CameraControlUI cameraControlUI;
    [SerializeField] private PauseMenuUI pauseMenu;

    [Header("HUD Elements")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private Text objectiveText;
    [SerializeField] private Text statusText;

    [Header("Mini-Map UI")]
    [SerializeField] private RawImage miniMapDisplay;
    [SerializeField] private Camera miniMapCameraComponent;

    public static GameUIManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        // Find components if not assigned
        if (detectionUI == null)
        {
            detectionUI = FindFirstObjectByType<DetectionUI>();
        }

        if (miniMapCamera == null)
        {
            miniMapCamera = FindFirstObjectByType<MiniMapCamera>();
        }

        if (cameraControlUI == null)
        {
            cameraControlUI = FindFirstObjectByType<CameraControlUI>();
        }

        if (pauseMenu == null)
        {
            pauseMenu = FindFirstObjectByType<PauseMenuUI>();
        }

        // Setup mini-map display
        if (miniMapDisplay != null && miniMapCameraComponent == null)
        {
            GameObject miniMapObj = GameObject.Find("MiniMapCamera");
            if (miniMapObj != null)
            {
                miniMapCameraComponent = miniMapObj.GetComponent<Camera>();
            }
        }

        if (miniMapDisplay != null && miniMapCameraComponent != null)
        {
            // Create render texture for mini-map
            RenderTexture miniMapRT = new RenderTexture(256, 256, 16);
            miniMapCameraComponent.targetTexture = miniMapRT;
            miniMapDisplay.texture = miniMapRT;
        }

        // Set default objective
        UpdateObjective("Escape the facility without being caught");
    }

    void Update()
    {
        UpdateStatusDisplay();
    }

    void UpdateStatusDisplay()
    {
        if (statusText == null) return;

        if (detectionUI != null)
        {
            if (detectionUI.IsCaught)
            {
                statusText.text = "STATUS: CAUGHT";
                statusText.color = Color.red;
            }
            else if (detectionUI.DetectionLevel > 0f)
            {
                statusText.text = $"STATUS: ALERT ({Mathf.RoundToInt(detectionUI.DetectionLevel * 100)}%)";
                statusText.color = Color.Lerp(Color.yellow, Color.red, detectionUI.DetectionLevel);
            }
            else
            {
                statusText.text = "STATUS: SAFE";
                statusText.color = Color.green;
            }
        }
    }

    // Public methods for other scripts to interact with UI
    public void UpdateObjective(string objective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"OBJECTIVE: {objective}";
        }
    }

    public void ShowNotification(string message, float duration = 3f)
    {
        // Could implement a notification system here
        Debug.Log($"Notification: {message}");
    }

    public void SetHUDVisible(bool visible)
    {
        if (hudPanel != null)
        {
            hudPanel.SetActive(visible);
        }
    }

    public DetectionUI GetDetectionUI() => detectionUI;
    public CameraControlUI GetCameraControlUI() => cameraControlUI;
    public MiniMapCamera GetMiniMapCamera() => miniMapCamera;
}
