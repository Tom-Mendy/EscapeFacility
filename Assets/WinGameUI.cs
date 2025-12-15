using UnityEngine;
using TMPro;

public class WinGameUI : MonoBehaviour
{
    [SerializeField] TMP_Text LifeText;
    [SerializeField] TMP_Text Score;
    [SerializeField] TMP_Text TimeMap1Text;
    [SerializeField] TMP_Text TimeMap2Text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMap2Chrono();

            if (LifeText != null)
                LifeText.text = "Life : " + GameManager.Instance.getLife().ToString();
            else
                Debug.LogWarning("WinGameUI: LifeText is not assigned in the inspector.");

             if (Score != null)
                Score.text = "Score : " + GameManager.Instance.score.ToString();
            else
                Debug.LogWarning("WinGameUI: Score is not assigned in the inspector.");

            if (TimeMap1Text != null)
                TimeMap1Text.text = "Time Map 1 : " + GameManager.Instance.getMap1Chrono().ToString("F2") + "s";
            else
                Debug.LogWarning("WinGameUI: TimeMap1Text is not assigned in the inspector.");

            if (TimeMap2Text != null)
                TimeMap2Text.text = "Time Map 2 : " + GameManager.Instance.getMap2Chrono().ToString("F2") + "s";
            else
                Debug.LogWarning("WinGameUI: TimeMap2Text is not assigned in the inspector.");
        }
        else
        {
            Debug.LogWarning("WinGameUI: GameManager.Instance is null.");
        }
    }
}
