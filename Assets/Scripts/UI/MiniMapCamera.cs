using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    private Transform player;
    [SerializeField] private float height = 20f;
    [SerializeField] private bool followPlayer = true;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSize = 10f;
    [SerializeField] private LayerMask miniMapLayers;

    private Camera miniMapCam;

    void Start()
    {
        // Get or create camera component
        miniMapCam = GetComponent<Camera>();
        if (miniMapCam == null)
        {
            miniMapCam = gameObject.AddComponent<Camera>();
        }

        // Configure camera for mini-map
        miniMapCam.orthographic = true;
        miniMapCam.orthographicSize = cameraSize;
        miniMapCam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("MiniMapCamera: Player not found. Assign player transform or tag player as 'Player'");
            }
        }

        // Position camera above player
        if (player != null)
        {
            UpdatePosition();
        }
    }

    void LateUpdate()
    {
        if (followPlayer && player != null)
        {
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        Vector3 newPosition = player.position;
        newPosition.y = player.position.y + height;
        transform.position = newPosition;
    }

    // Public method to set camera size (zoom level)
    public void SetZoom(float size)
    {
        if (miniMapCam != null)
        {
            miniMapCam.orthographicSize = Mathf.Clamp(size, 5f, 50f);
        }
    }

    // Public method to toggle follow mode
    public void SetFollowPlayer(bool follow)
    {
        followPlayer = follow;
    }
}
