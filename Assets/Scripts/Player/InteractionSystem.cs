using UnityEngine;
using TMPro;

public class Interactions : MonoBehaviour
{
    public TMP_Text interactionText;
    private Door currentDoor;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Door>() != null)
        {
            currentDoor = other.GetComponentInParent<Door>();
            interactionText.text = currentDoor.GetInteractionText();
            interactionText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Door>() != null)
        {
            currentDoor = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (currentDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            currentDoor.Interact();
            interactionText.text = currentDoor.GetInteractionText();
        }
    }
}
