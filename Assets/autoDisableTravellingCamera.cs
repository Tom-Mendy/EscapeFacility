using UnityEngine;

public class autoDisableTravellingCamera : MonoBehaviour
{
    public GameObject playerCamera;
    void Start() {
        playerCamera.SetActive(false);
    }
    void disableCamera()
    {
        playerCamera.SetActive(true);
        gameObject.SetActive(false);
    }
}