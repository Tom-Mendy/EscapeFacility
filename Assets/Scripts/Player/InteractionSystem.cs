using TMPro;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public TMP_Text interactionText;
    private Door currentDoor;
    private Lever currentLever;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Door>() != null)
        {
            currentDoor = other.GetComponentInParent<Door>();
            interactionText.text = currentDoor.GetInteractionText();
            interactionText.gameObject.SetActive(true);
        }
        if (other.gameObject.GetComponentInParent<Lever>() != null)
        {
            currentLever = other.GetComponentInParent<Lever>();
            interactionText.text = currentLever.GetInteractionText();
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
        if (other.gameObject.GetComponentInParent<Lever>() != null)
        {
            currentLever = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentDoor != null)
            {
                currentDoor.Interact();
                interactionText.text = currentDoor.GetInteractionText();
            }

            if (currentLever != null)
            {
                currentLever.Interact();
                interactionText.text = currentLever.GetInteractionText();
            }
        }
    }
}
