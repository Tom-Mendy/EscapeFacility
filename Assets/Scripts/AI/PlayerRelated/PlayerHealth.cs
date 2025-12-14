using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Invincibility")]
    public float invincibilityTime = 1.5f;

    private bool isInvincible = false;

    public void LoseLife()
    {
        if (isInvincible)
            return;

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
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    void ReturnToMainMenu()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("MainMenu");
    }

    System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}
