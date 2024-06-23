using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int collectedDiamonds;
    [SerializeField] private int winCondition = 3;
    [SerializeField] private SceneFader sceneFader; // Tambahkan ini
    [SerializeField] private string nextSceneName; // Tambahkan ini untuk mengganti scene di inspector

    private static GameManager instance;
    public GameObject gameOverUI;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).Name);
                    instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton); // Optional: Jika ingin tetap ada di antara pergantian scene
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Jika ingin tetap ada di antara pergantian scene
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Start()
    {
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }

    public void AddDiamonds(int _diamonds)
    {
        collectedDiamonds += _diamonds;
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }

    public void Finish()
    {
        Debug.Log("Finish method called");
        if (collectedDiamonds >= winCondition)
        {
            Debug.Log("Collected Items: " + collectedDiamonds + ". Loading " + nextSceneName);
            if (sceneFader != null)
            {
                HealthManager healthManager = FindObjectOfType<HealthManager>();
                if (healthManager != null)
                {
                    healthManager.ResetHealth();
                }
                sceneFader.FadeOutAndLoadScene(nextSceneName);
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            Debug.Log("Not enough Items. Collected: " + collectedDiamonds + " / " + winCondition);
            UIManager.MyInstance.ShowWinCondition(collectedDiamonds, winCondition);
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver method called"); // Tambahkan log ini
        if (gameOverUI != null)
        {
            Debug.Log("Activating gameOverUI"); // Tambahkan log ini
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogError("gameOverUI is not set in the inspector"); // Tambahkan log ini
        }
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        gameOverUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene

        StartCoroutine(ResetHealthAfterSceneLoad()); // Reset health bar after the scene is loaded
    }

    private IEnumerator ResetHealthAfterSceneLoad()
    {
        yield return null; // Wait for the scene to fully load
        HealthManager healthManager = FindObjectOfType<HealthManager>();
        if (healthManager != null)
        {
            healthManager.ResetHealth();
        }
        UIManager.MyInstance.ResetHealthBar(); // Reset health bar value
    }

    public void LoadHomeScene()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Home"); // Replace "Home" with the actual name of your home scene
    }
}
