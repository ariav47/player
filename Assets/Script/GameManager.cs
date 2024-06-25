using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int collectedDiamonds;
    [SerializeField] private int winCondition = 3;
    [SerializeField] private string nextScene; // Next scene name

    private static GameManager instance;
    public GameObject gameOverUI;
    [SerializeField] private SceneFader sceneFader;

    [SerializeField] private AudioClip diamondCollectSound; // Tambahkan referensi ke AudioClip melalui SerializeField
    private AudioSource audioSource; // Tambahkan referensi ke AudioSource

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

        audioSource = GetComponent<AudioSource>(); // Inisialisasi AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Tambahkan AudioSource jika tidak ada
        }
    }

    private void Start()
    {
        SetWinConditionForLevel();
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }

    private void SetWinConditionForLevel()
    {
        // Reset collected diamonds
        collectedDiamonds = 0;

        // Set win condition and next scene based on the current level
        switch (SceneManager.GetActiveScene().name)
        {
            case "Char":
                winCondition = 3;
                nextScene = "Level 2";
                break;
            case "Level 2":
                winCondition = 5;
                nextScene = "Level 3";
                break;
            case "Level 3":
                winCondition = 3;
                nextScene = "Ending"; // Set next scene for level 3
                break;
            default:
                winCondition = 3;
                nextScene = "Char"; // Default next scene
                break;
        }

        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name + ", Win Condition: " + winCondition + ", Next Scene: " + nextScene);
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }

    public void AddDiamonds(int _diamonds)
    {
        collectedDiamonds += _diamonds;
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);

        // Putar suara jika ada AudioSource dan AudioClip
        if (audioSource != null && diamondCollectSound != null)
        {
            audioSource.PlayOneShot(diamondCollectSound);
        }
        else
        {
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is not set.");
            }
            if (diamondCollectSound == null)
            {
                Debug.LogError("diamondCollectSound is not set.");
            }
        }
    }

    public void Finish()
    {
        Debug.Log("Finish method called");
        if (collectedDiamonds >= winCondition)
        {
            Debug.Log("Collected Items: " + collectedDiamonds + ". Loading " + nextScene);
            sceneFader.FadeOutAndLoadScene(nextScene);
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
        UIManager.MyInstance.ResetHealthBar(); // Reset health bar value
    }

    public void LoadHomeScene()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Home"); // Replace "Home" with the actual name of your home scene
    }

    private void OnLevelWasLoaded(int level)
    {
        SetWinConditionForLevel();
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }
}