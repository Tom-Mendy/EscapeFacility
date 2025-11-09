using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private string cameraName = "Camera 1";
    [SerializeField] private bool isActive = true;
    [SerializeField] private Camera cameraComponent;

    [Header("Rotation Settings")]
    [SerializeField] private bool canRotate = true;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float rotationAngle = 60f;
    [SerializeField] private Transform rotationPivot;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float detectionFOV = 90f;
    [SerializeField] private Color detectionColor = Color.green;
    [SerializeField] private Color alertColor = Color.red;

    [Header("Visual Feedback")]
    [SerializeField] private Light cameraLight;
    [SerializeField] private Renderer cameraRenderer;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private float currentRotation = 0f;
    private bool rotatingRight = true;
    private bool playerDetected = false;

    public string CameraName => cameraName;
    public bool IsActive => isActive;
    public Camera CameraComponent => cameraComponent;

    void Start()
    {
        // Get camera component if not assigned
        if (cameraComponent == null)
        {
            cameraComponent = GetComponentInChildren<Camera>();
        }

        // Setup rotation pivot
        if (rotationPivot == null)
        {
            rotationPivot = transform;
        }

        // Initialize camera state
        UpdateCameraState();
    }

    void Update()
    {
        if (!isActive) return;

        if (canRotate)
        {
            RotateCamera();
        }

        DetectPlayer();
    }

    void RotateCamera()
    {
        // Oscillate rotation
        if (rotatingRight)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            if (currentRotation >= rotationAngle)
            {
                currentRotation = rotationAngle;
                rotatingRight = false;
            }
        }
        else
        {
            currentRotation -= rotationSpeed * Time.deltaTime;
            if (currentRotation <= -rotationAngle)
            {
                currentRotation = -rotationAngle;
                rotatingRight = true;
            }
        }

        rotationPivot.localRotation = Quaternion.Euler(0f, currentRotation, 0f);
    }

    void DetectPlayer()
    {
        playerDetected = false;

        // Check for player in detection cone
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, detectionLayers);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Vector3 directionToPlayer = (col.transform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(rotationPivot.forward, directionToPlayer);

                if (angleToPlayer < detectionFOV * 0.5f)
                {
                    // Raycast to check line of sight
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
                    {
                        if (hit.collider == col)
                        {
                            playerDetected = true;
                            OnPlayerDetected();
                            break;
                        }
                    }
                }
            }
        }

        UpdateVisualFeedback();
    }

    void OnPlayerDetected()
    {
        // Alert AI system or trigger alarm
        Debug.Log($"{cameraName}: Player detected!");

        // Could trigger AIManager event here
        // AIManager.Instance?.PlayerSpotted(playerTransform);
    }

    void UpdateVisualFeedback()
    {
        Color targetColor = playerDetected ? alertColor : detectionColor;

        if (cameraLight != null)
        {
            cameraLight.color = targetColor;
        }
    }

    void UpdateCameraState()
    {
        if (cameraComponent != null)
        {
            cameraComponent.enabled = isActive;
        }

        if (cameraLight != null)
        {
            cameraLight.enabled = isActive;
        }

        if (cameraRenderer != null)
        {
            cameraRenderer.material = isActive ? activeMaterial : inactiveMaterial;
        }
    }

    // Public method to toggle camera
    public void ToggleCamera()
    {
        SetCameraActive(!isActive);
    }

    // Public method to set camera state
    public void SetCameraActive(bool active)
    {
        isActive = active;
        UpdateCameraState();
    }

    void OnDrawGizmosSelected()
    {
        if (rotationPivot == null) rotationPivot = transform;

        Gizmos.color = playerDetected ? alertColor : detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw detection cone
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionFOV * 0.5f, 0) * rotationPivot.forward * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionFOV * 0.5f, 0) * rotationPivot.forward * detectionRange;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rotationPivot.forward * detectionRange);
    }
}
