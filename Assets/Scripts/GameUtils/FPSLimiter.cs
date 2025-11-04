using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0; // Disable VSync (important!)
        Application.targetFrameRate = 60; // Set your FPS cap here
    }
}
