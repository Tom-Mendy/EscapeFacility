using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    public WinGameUIManager trigger;

    private void OnTriggerEnter(Collider other)
    {
        trigger.WinGame();
    }
}
