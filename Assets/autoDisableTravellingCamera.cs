using UnityEngine;

public class autoDisableTravellingCamera : MonoBehaviour
{
    void disableCamera()
    {
        this.GetComponent<Camera>().enabled = false;
    }
}