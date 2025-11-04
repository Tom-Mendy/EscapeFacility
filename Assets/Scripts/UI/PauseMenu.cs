using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject pauseGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseGame == null)
        {
            Debug.LogError("Must reference the Pause Game Object");
        }
        if (resumeButton == null)
        {
            Debug.LogError("Must reference the Resume Button");
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(SwitchState);
        }
    }

    // FixedUpdate is called at a fixed interval and is commonly used for physics updates
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState();
        }
    }

    void SwitchState()
    {
        pauseGame.SetActive(!pauseGame.activeSelf);
        if (pauseGame.activeSelf)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

}
