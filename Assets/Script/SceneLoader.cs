using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Ganti "Home" dengan nama scene menu utama Anda
    public string firstSceneName = "Home"; 

    void Start()
    {
        SceneManager.LoadScene(firstSceneName);
    }
}