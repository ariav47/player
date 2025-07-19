using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string EnterScene;
    public string EscapeScene;
    public bool isEscapeForQuit = false;

    public AudioSource backgroundAudio; // Add this field for background audio

    public static MainMenu MyInstance; // Ganti NAMA_SCRIPT dengan UIManager atau MainMenu

    private void Awake()
    {
        // HANYA INI. Tidak ada DontDestroyOnLoad atau Destroy.
        MyInstance = this;
    }

    private void Start()
    {
        if (backgroundAudio != null && !backgroundAudio.isPlaying)
        {
            backgroundAudio.Play();
        }
    }

	void Update()
	{
	    if (Input.GetKeyUp(KeyCode.Return))
	    {
	        Debug.Log("Name Scene: " + EnterScene);
	        SceneManager.LoadScene(EnterScene);
	    }
	
	    if (Input.GetKeyUp(KeyCode.Escape))
	    {
	        if (isEscapeForQuit)
	        {
	            Application.Quit();
	        }
	        else
	        {
	            Debug.Log("Name Scene: " + EscapeScene);
	            SceneManager.LoadScene(EscapeScene);
	        }
	    }
	}

    public void LevelGame()
    {
        Debug.Log("PERINTAH: Memulai proses loading scene 'Level' SEKARANG...");
        SceneManager.LoadScene("Level");
    }

    public void GoToCredit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("Home");     
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Exit");
    }

    public void GoToLevel(string sceneName)
    {
        // Cek apakah LevelManager yang abadi ada
        if (LevelManager.MyInstance != null)
        {
            // Perintahkan LevelManager untuk memuat scene yang diminta
            LevelManager.MyInstance.LoadToScene(sceneName);
        }
        else
        {
            // Fallback jika LevelManager tidak ditemukan
            Debug.LogError("LevelManager.MyInstance not found! Cannot load scene.");
        }
    }
}
