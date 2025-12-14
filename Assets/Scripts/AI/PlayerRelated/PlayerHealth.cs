using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public void LoseLife()
    {

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        GameManager.Instance.life--;
        Debug.Log("Player hit! Lives left: " + GameManager.Instance.life);

        if (GameManager.Instance.life <= 0)
        {
            ReturnToMainMenu();
        }
        else
        {
            SceneManager.LoadScene("Level2");
        }
    }

    void ReturnToMainMenu()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("MainMenu");
    }
}
