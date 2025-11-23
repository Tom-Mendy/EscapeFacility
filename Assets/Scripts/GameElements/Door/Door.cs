using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    public bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    public string GetInteractionText()
    {
        return isOpen ? "Press E to Close" : "Press E to Open";
    }
}
