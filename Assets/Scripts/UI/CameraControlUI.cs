using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControlUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject cameraControlPanel;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject cameraButtonPrefab;

    [Header("Camera Display")]
    [SerializeField] private RawImage cameraDisplayImage;
    [SerializeField] private Text cameraNameText;

    [Header("Security Cameras")]
    [SerializeField] private SecurityCamera[] securityCameras;

    private List<Button> cameraButtons = new List<Button>();
    private int currentCameraIndex = -1;
    private Dictionary<Button, SecurityCamera> buttonCameraMap = new Dictionary<Button, SecurityCamera>();

    void Start()
    {
        // Find all security cameras if not assigned
        if (securityCameras == null || securityCameras.Length == 0)
        {
            securityCameras = FindObjectsByType<SecurityCamera>(FindObjectsSortMode.None);
        }

        InitializeCameraButtons();

        // Hide panel by default
        if (cameraControlPanel != null)
        {
            cameraControlPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Toggle camera control panel with Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCameraPanel();
        }

        // Cycle through cameras with number keys
        for (int i = 0; i < Mathf.Min(securityCameras.Length, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectCamera(i);
            }
        }
    }

    void InitializeCameraButtons()
    {
        if (buttonContainer == null || cameraButtonPrefab == null)
        {
            Debug.LogWarning("CameraControlUI: Button container or prefab not assigned");
            return;
        }

        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        cameraButtons.Clear();
        buttonCameraMap.Clear();

        // Create button for each camera
        for (int i = 0; i < securityCameras.Length; i++)
        {
            SecurityCamera camera = securityCameras[i];
            if (camera == null) continue;

            GameObject buttonObj = Instantiate(cameraButtonPrefab, buttonContainer);
            Button button = buttonObj.GetComponent<Button>();

            if (button != null)
            {
                // Setup button text
                Text buttonText = buttonObj.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = camera.CameraName;
                }

                // Setup toggle functionality
                int index = i;
                button.onClick.AddListener(() => ToggleCameraAtIndex(index));

                cameraButtons.Add(button);
                buttonCameraMap[button] = camera;

                // Update button visual state
                UpdateButtonVisual(button, camera);
            }
        }
    }

    void ToggleCameraAtIndex(int index)
    {
        if (index >= 0 && index < securityCameras.Length)
        {
            SecurityCamera camera = securityCameras[index];
            camera.ToggleCamera();

            // Update button visual
            if (index < cameraButtons.Count)
            {
                UpdateButtonVisual(cameraButtons[index], camera);
            }
        }
    }

    void SelectCamera(int index)
    {
        if (index >= 0 && index < securityCameras.Length)
        {
            currentCameraIndex = index;
            UpdateCameraDisplay();
        }
    }

    void UpdateCameraDisplay()
    {
        if (currentCameraIndex < 0 || currentCameraIndex >= securityCameras.Length)
        {
            if (cameraDisplayImage != null)
            {
                cameraDisplayImage.texture = null;
            }
            if (cameraNameText != null)
            {
                cameraNameText.text = "No Camera Selected";
            }
            return;
        }

        SecurityCamera camera = securityCameras[currentCameraIndex];

        if (cameraNameText != null)
        {
            cameraNameText.text = camera.CameraName;
        }

        if (cameraDisplayImage != null && camera.CameraComponent != null)
        {
            // Create render texture if needed
            if (camera.CameraComponent.targetTexture == null)
            {
                RenderTexture rt = new RenderTexture(512, 512, 16);
                camera.CameraComponent.targetTexture = rt;
            }

            cameraDisplayImage.texture = camera.CameraComponent.targetTexture;
        }
    }

    void UpdateButtonVisual(Button button, SecurityCamera camera)
    {
        if (button == null) return;

        // Change button color based on camera state
        ColorBlock colors = button.colors;
        colors.normalColor = camera.IsActive ? Color.green : Color.red;
        button.colors = colors;

        // Update button text if needed
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = $"{camera.CameraName} {(camera.IsActive ? "[ON]" : "[OFF]")}";
        }
    }

    void ToggleCameraPanel()
    {
        if (cameraControlPanel != null)
        {
            bool newState = !cameraControlPanel.activeSelf;
            cameraControlPanel.SetActive(newState);

            // Toggle cursor
            if (newState)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    // Public method to refresh all camera buttons
    public void RefreshCameraButtons()
    {
        for (int i = 0; i < cameraButtons.Count && i < securityCameras.Length; i++)
        {
            UpdateButtonVisual(cameraButtons[i], securityCameras[i]);
        }
    }

    // Public method to get all cameras
    public SecurityCamera[] GetSecurityCameras()
    {
        return securityCameras;
    }
}
