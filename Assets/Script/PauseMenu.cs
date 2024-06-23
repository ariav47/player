using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        // Reset HealthManager
        HealthManager healthManager = FindObjectOfType<HealthManager>();
        if (healthManager != null)
        {
            healthManager.Respawn();
        }

        // Reset UIManager health bar
        UIManager uiManager = UIManager.MyInstance;
        if (uiManager != null)
        {
            uiManager.ResetHealthBar();
        }

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Home()
    {
        SceneManager.LoadScene("Home");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
