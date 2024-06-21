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
}
