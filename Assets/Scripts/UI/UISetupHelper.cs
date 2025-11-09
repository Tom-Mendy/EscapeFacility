using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Utilitaire pour configurer automatiquement les composants UI du jeu
/// Utilisez ceci en mode éditeur pour générer rapidement la structure UI de base
/// </summary>
public class UISetupHelper : MonoBehaviour
{
    [Header("Auto-Setup Options")]
    [SerializeField] private bool createDetectionUI = true;
    [SerializeField] private bool createCaughtScreen = true;
    [SerializeField] private bool createMiniMap = true;
    [SerializeField] private bool createCameraControl = true;

    [Header("References")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Transform player;

    [ContextMenu("Setup UI Components")]
    public void SetupUIComponents()
    {
        if (mainCanvas == null)
        {
            Debug.LogError("Main Canvas not assigned!");
            return;
        }

        if (createDetectionUI)
        {
            CreateDetectionBar();
        }

        if (createCaughtScreen)
        {
            CreateCaughtScreen();
        }

        if (createMiniMap)
        {
            CreateMiniMap();
        }

        if (createCameraControl)
        {
            CreateCameraControlPanel();
        }

        Debug.Log("UI Setup Complete!");
    }

    void CreateDetectionBar()
    {
        // Create detection bar panel
        GameObject detectionPanel = new GameObject("DetectionBarPanel");
        detectionPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform panelRect = detectionPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.9f);
        panelRect.anchorMax = new Vector2(0.5f, 0.9f);
        panelRect.sizeDelta = new Vector2(400f, 30f);

        Image panelBg = detectionPanel.AddComponent<Image>();
        panelBg.color = new Color(0, 0, 0, 0.5f);

        // Create fill image
        GameObject fillObj = new GameObject("FillImage");
        fillObj.transform.SetParent(detectionPanel.transform, false);

        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(5, 5);
        fillRect.offsetMax = new Vector2(-5, -5);

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillAmount = 0f;
        fillImage.color = Color.yellow;

        detectionPanel.SetActive(false);

        Debug.Log("Detection Bar created!");
    }

    void CreateCaughtScreen()
    {
        // Create caught panel
        GameObject caughtPanel = new GameObject("CaughtPanel");
        caughtPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform caughtRect = caughtPanel.AddComponent<RectTransform>();
        caughtRect.anchorMin = Vector2.zero;
        caughtRect.anchorMax = Vector2.one;
        caughtRect.offsetMin = Vector2.zero;
        caughtRect.offsetMax = Vector2.zero;

        Image caughtBg = caughtPanel.AddComponent<Image>();
        caughtBg.color = new Color(1f, 0f, 0f, 0.5f);

        // Create caught text
        GameObject textObj = new GameObject("CaughtText");
        textObj.transform.SetParent(caughtPanel.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(400f, 100f);

        Text caughtText = textObj.AddComponent<Text>();
        caughtText.text = "CAUGHT!";
        caughtText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        caughtText.fontSize = 72;
        caughtText.alignment = TextAnchor.MiddleCenter;
        caughtText.color = Color.white;

        caughtPanel.SetActive(false);

        Debug.Log("Caught Screen created!");
    }

    void CreateMiniMap()
    {
        // Create mini-map display
        GameObject miniMapObj = new GameObject("MiniMapDisplay");
        miniMapObj.transform.SetParent(mainCanvas.transform, false);

        RectTransform miniMapRect = miniMapObj.AddComponent<RectTransform>();
        miniMapRect.anchorMin = new Vector2(1f, 1f);
        miniMapRect.anchorMax = new Vector2(1f, 1f);
        miniMapRect.pivot = new Vector2(1f, 1f);
        miniMapRect.anchoredPosition = new Vector2(-10f, -10f);
        miniMapRect.sizeDelta = new Vector2(200f, 200f);

        RawImage miniMapImage = miniMapObj.AddComponent<RawImage>();
        miniMapImage.color = Color.white;

        // Create mini-map camera
        GameObject camObj = new GameObject("MiniMapCamera");
        Camera miniCam = camObj.AddComponent<Camera>();
        miniCam.orthographic = true;
        miniCam.orthographicSize = 10f;
        miniCam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        MiniMapCamera miniMapScript = camObj.AddComponent<MiniMapCamera>();
        if (player != null)
        {
            miniMapScript.GetType().GetField("player",
      System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
       ?.SetValue(miniMapScript, player);
        }

        // Create render texture
        RenderTexture rt = new RenderTexture(256, 256, 16);
        miniCam.targetTexture = rt;
        miniMapImage.texture = rt;

        Debug.Log("Mini-Map created!");
    }

    void CreateCameraControlPanel()
    {
        // Create camera control panel
        GameObject controlPanel = new GameObject("CameraControlPanel");
        controlPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform panelRect = controlPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 0.5f);
        panelRect.anchorMax = new Vector2(0f, 0.5f);
        panelRect.pivot = new Vector2(0f, 0.5f);
        panelRect.anchoredPosition = new Vector2(10f, 0f);
        panelRect.sizeDelta = new Vector2(250f, 400f);

        Image panelBg = controlPanel.AddComponent<Image>();
        panelBg.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

        // Create camera display
        GameObject displayObj = new GameObject("CameraDisplay");
        displayObj.transform.SetParent(controlPanel.transform, false);

        RectTransform displayRect = displayObj.AddComponent<RectTransform>();
        displayRect.anchorMin = new Vector2(0.5f, 1f);
        displayRect.anchorMax = new Vector2(0.5f, 1f);
        displayRect.pivot = new Vector2(0.5f, 1f);
        displayRect.anchoredPosition = new Vector2(0f, -10f);
        displayRect.sizeDelta = new Vector2(230f, 150f);

        RawImage displayImage = displayObj.AddComponent<RawImage>();
        displayImage.color = Color.gray;

        // Create camera name text
        GameObject nameObj = new GameObject("CameraNameText");
        nameObj.transform.SetParent(controlPanel.transform, false);

        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0.5f, 1f);
        nameRect.anchorMax = new Vector2(0.5f, 1f);
        nameRect.pivot = new Vector2(0.5f, 1f);
        nameRect.anchoredPosition = new Vector2(0f, -170f);
        nameRect.sizeDelta = new Vector2(230f, 30f);

        Text nameText = nameObj.AddComponent<Text>();
        nameText.text = "No Camera Selected";
        nameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        nameText.fontSize = 16;
        nameText.alignment = TextAnchor.MiddleCenter;
        nameText.color = Color.white;

        // Create button container
        GameObject containerObj = new GameObject("ButtonContainer");
        containerObj.transform.SetParent(controlPanel.transform, false);

        RectTransform containerRect = containerObj.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0f);
        containerRect.anchorMax = new Vector2(0.5f, 1f);
        containerRect.pivot = new Vector2(0.5f, 1f);
        containerRect.anchoredPosition = new Vector2(0f, -210f);
        containerRect.sizeDelta = new Vector2(230f, 180f);

        VerticalLayoutGroup layout = containerObj.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 5f;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        controlPanel.SetActive(false);

        Debug.Log("Camera Control Panel created!");
    }

    [ContextMenu("Find and Assign References")]
    public void FindReferences()
    {
        if (mainCanvas == null)
        {
            mainCanvas = FindFirstObjectByType<Canvas>();
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        Debug.Log($"Canvas: {(mainCanvas != null ? "Found" : "Not Found")}");
        Debug.Log($"Player: {(player != null ? "Found" : "Not Found")}");
    }
}
