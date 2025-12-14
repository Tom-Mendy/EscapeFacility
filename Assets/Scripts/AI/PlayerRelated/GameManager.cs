using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    float Map1Time = 0;
    float Map1TimeStart = 0;
    float Map2Time = 0;
    float Map2TimeStart = 0;
    public int life = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks or duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When Map2 is loaded, start the Map1 chrono
        if (scene.name == "Level2" && Map1TimeStart == 0)
        {
            SetMap1ChronoStart();
        }
        // When Map2 is loaded, start the Map1 chrono
        if (scene.name == "Level3" && Map2TimeStart == 0)
        {
            SetMap1Chrono();
            SetMap2ChronoStart();
        }
    }

    public void ResetGame()
    {
        score = 0;
        Map1Time = 0;
        Map1TimeStart = 0;
        Map2Time = 0;
        Map2TimeStart = 0;
        life = 3;
    }

    void SetMap1ChronoStart()
    {
        Map1TimeStart = Time.time;
    }

    void SetMap1Chrono()
    {
        Map1Time = Time.time - Map1TimeStart;
    }

    public float getMap1Chrono()
    {
        return Map1Time;
    }

    void SetMap2ChronoStart()
    {
        Map2TimeStart = Time.time;
    }

    public void SetMap2Chrono()
    {
        Map2Time = Time.time - Map2TimeStart;
    }


    public float getMap2Chrono()
    {
        return Map2Time;
    }

    public int getLife()
    {
        return life;
    }

    public int setLife(int damage)
    {
        return life - damage;
    }

}
