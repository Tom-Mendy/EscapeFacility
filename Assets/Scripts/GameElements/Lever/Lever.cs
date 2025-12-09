using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator animator;
    public bool isOn = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        isOn = !isOn;
        animator.SetBool("LeverUp", isOn);

        // Can trigger other game events here based on lever state
        Debug.Log("Lever toggled: " + isOn);
    }

    public string GetInteractionText()
    {
        return isOn ? "Press E to Turn Off" : "Press E to Turn On";
    }
}
