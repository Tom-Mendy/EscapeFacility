using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button playButton;

    void Start()
    {
        if (exitButton == null)
        {
            Debug.LogError("Must reference the Exit Button");
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }
        if (playButton == null)
        {
            Debug.LogError("Must reference the Exit Button");
        }
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
        }
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    private void OnPlayButtonClick()
    {
        SceneManager.LoadScene("Level2");
        Debug.Log("Switch scene to Level2 one");
    }
}
