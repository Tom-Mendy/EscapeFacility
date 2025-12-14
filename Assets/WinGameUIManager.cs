using UnityEngine;

public class WinGameUIManager : MonoBehaviour
{
    [SerializeField] GameObject winGamePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winGamePanel.SetActive(false);
    }

    public void WinGame()
    {
         winGamePanel.SetActive(true);
    }
}
