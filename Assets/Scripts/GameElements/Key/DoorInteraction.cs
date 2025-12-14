using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{

    [SerializeField] private Animator doorAnimator;
    public KeyType requiredKey;
    private bool isOpen = false;

    private void OnEnable()
    {
        PlayerKeyInventory.OnKeyCollected += HandleKeyCollected;
    }

    private void OnDisable()
    {
        PlayerKeyInventory.OnKeyCollected -= HandleKeyCollected;
    }

    void HandleKeyCollected(KeyType key)
    {
        if (isOpen)
            return;

        if (key == requiredKey)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        Debug.Log("Door opened automatically: " + requiredKey);

        // Your door behavior here
        gameObject.SetActive(false);
        if (PickUpKey.Instance.IsKeyPickedUp())
        {
            if (requiredKey == KeyType.Red)
                doorAnimator.SetTrigger("IsDoorOpen");
            else if (requiredKey == KeyType.Blue)
                doorAnimator.SetTrigger("IsDoorOpen2");
            else if (requiredKey == KeyType.Green)
                doorAnimator.SetTrigger("IsDoorOpen3");
        }
        // OR play animation
    }

    //private void FixedUpdate()
    //{
    //    Debug.Log("Key picked up: " + PickUpKey.Instance.IsKeyPickedUp());
    //    if (PickUpKey.Instance.IsKeyPickedUp())
    //    {
    //        //if (PickUpKey.Instance.GetKeyNumber() == 2)
    //        //    doorAnimator.SetTrigger("IsDoorOpen");
    //        //if (PickUpKey.Instance.GetKeyNumber() == 3)
    //        //    doorAnimator.SetTrigger("IsDoorOpen2");
    //        //if (PickUpKey.Instance.GetKeyNumber() == 4)
    //        //    doorAnimator.SetTrigger("IsDoorOpen3");
    //    }
    //}
}
